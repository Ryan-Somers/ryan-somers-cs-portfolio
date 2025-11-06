using System.Collections;
using System.Text.Json.Serialization;
using AutoMapper;
using HFA.Login;
using HFA.Models;
using System.Security.Claims;
using System.ServiceModel;
using HFA.Models.ViewModels;
using HFA.Resources;
using HFA.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Channels;
using System;
using Microsoft.IdentityModel.Protocols.WsTrust;
using Newtonsoft.Json;

namespace HFA.Controllers
{
    [SessionAuthorize(["EXE", "DEV"])]
    public class ContentHubController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IDbAccess _dbAccess;
        private readonly IMapper _mapper;
        private readonly LoginSoapClient _loginClient;
        private readonly LoginServiceOptionsProvider _optionsProvider;

        public ContentHubController(IDbAccess dbAccess, IMapper mapper, IImageService imageService, LoginServiceOptionsProvider optionsProvider) {
            _imageService = imageService;
            _dbAccess = dbAccess;
            _mapper = mapper;
            BasicHttpsBinding binding = new();
            EndpointAddress uri = new(optionsProvider.EndpointUri);
            _loginClient = new LoginSoapClient(binding, uri);
            _optionsProvider = optionsProvider;
        }

        [SessionAuthorize(["EXE", "DEV"])]
        public IActionResult Index()
        {
            return View();
        }

        #region FAQ
        [SessionAuthorize(["EXE"])]
        [HttpGet]
        public async Task<IActionResult> FAQ()
        {
            try
            {
                return View(await _dbAccess.GetFAQAsync());
            } catch
            {
                return StatusCode(500);
            }
        }
        
        // PEA Notes:
        // // Ensured that I used the correct HTTP methods for the controller methods.
        // // I created a few new controller methods so that the user can Create/Update/Delete the FAQ.
        // // I also added a new view for the FAQ page.
        // // From the lines 66 - 150 are my contribution. 
        
        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/CreateFAQ")]
        public async Task<IActionResult> CreateFAQ()
        {
            return View();
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost]
        public async Task<IActionResult> CreateFAQ(FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            if (!ModelState.IsValid)
            {
                // Return the form with validation errors
                return View(frequentlyAskedQuestion);
            }

            await _dbAccess.CreateFAQAsync(frequentlyAskedQuestion);

            // Redirect to the FAQ list
            return RedirectToAction("FAQ", "ContentHub");

        }
        
        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/UpdateFAQ")]
        public async Task<IActionResult> UpdateFAQ(int id)
        {
            var faq = await _dbAccess.GetFAQByIdAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost]
        public async Task<IActionResult> UpdateFAQ(FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            if (!ModelState.IsValid)
            {
                return View(frequentlyAskedQuestion);
            }

            try
            {
                await _dbAccess.UpdateFAQAsync(frequentlyAskedQuestion);
                return RedirectToAction("FAQ", "ContentHub");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(frequentlyAskedQuestion);
            }
        }
        
        [SessionAuthorize(["EXE"])]
        [HttpGet]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var faq = await _dbAccess.GetFAQByIdAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }


        [SessionAuthorize(["EXE"])]
        [HttpPost]
        public async Task<IActionResult> DeleteFAQ(FrequentlyAskedQuestion frequentlyAskedQuestion)
        {
            try
            {
                await _dbAccess.DeleteFAQAsync(frequentlyAskedQuestion.FaqId);
                return RedirectToAction("FAQ");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(frequentlyAskedQuestion);
            }
        }


        #endregion

        #region Articles
        [SessionAuthorize(["EXE"])]
        public async Task<IActionResult> Articles()
        {
            try
            {
                var articles = await _dbAccess.GetArticlesAsync();
                var articleSummaryViewModels = _mapper.Map<List<ArticleSummaryViewModel>>(articles);
                var culture = HFA_Resources.Culture;

                return View(articles);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/Articles/CreateArticle")]
        public IActionResult CreateArticle()
        {
            ViewBag.Action = "action-create";
            return View("ArticleEditPage", new ArticleEditViewModel());
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Articles/CreateArticle")]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleEditViewModel articleEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState Errors:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return BadRequest(ModelState);
            }

            try
            {
                Article article = articleEditViewModel.Article;
        
                // Retrieve the selected language from the form data
                var selectedLanguage = Request.Form["Language"];

                // Set content fields based on the selected language
                    article.TitleFR = Request.Form["Article.TitleFR"];
                    article.ContentBodyFR = Request.Form["Article.ContentBodyFR"];
                    article.Title = Request.Form["Article.Title"];
                    article.ContentBody = Request.Form["Article.ContentBody"];
                    article.Lead = Request.Form["Article.Lead"];
                    article.LeadFR = Request.Form["Article.LeadFR"];

                if (articleEditViewModel.HeroImageId != null)
                {
                    article.HeroImageId = articleEditViewModel.HeroImageId;
                }

                var claimNameValue = User.Claims.First(c => c.Type == "name").Value;

                if (article.Author == null)
                {
                    article.Author = claimNameValue;
                    article.CreatedDate = DateTime.Now;
                }

                article.MostRecentEditor = claimNameValue;
                article.LastModifiedDate = DateTime.Now;

                await _dbAccess.UpdateArticleAsync(article);

                return Ok(article.ArticleId);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/Articles/UpdateArticle/{id}")]
        public async Task<IActionResult> UpdateArticle(int id)
        {
            try
            {
                ViewBag.Action = "action-update";

                var article = await _dbAccess.GetArticleAsync(id);
                if (article == null)
                {
                    return NotFound(); // 404 if the article does not exist
                }

                // Prepare the ViewModel
                var articleEditViewModel = new ArticleEditViewModel(article, article.HeroImageId);

                // Render the edit page
                return View("ArticleEditPage", articleEditViewModel);
            }
            catch
            {
                return StatusCode(500); // Internal server error
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("[controller]/Articles/UpdateArticle")]
        public async Task<IActionResult> UpdateArticle([FromForm] ArticleEditViewModel articleEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-update";
                return BadRequest(ModelState);
            }

            try
            {
                Article article = articleEditViewModel.Article;
                if (articleEditViewModel.HeroImageId != null)
                {
                    article.HeroImageId = articleEditViewModel.HeroImageId;
                }

                // Update log values
                var claimNameValue = User.Claims.First(c => c.Type == "name").Value;

                if (article.Author == null)
                {
                    article.Author ??= claimNameValue;
                    article.CreatedDate ??= DateTime.Now;
                }

                article.MostRecentEditor = claimNameValue;
                article.LastModifiedDate = DateTime.Now;

                await _dbAccess.UpdateArticleAsync(article);

                return Ok(articleEditViewModel.Article.ArticleId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost]
        [Route("[controller]/DeleteArticle/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                await _dbAccess.DeleteArticleAsync(id);

                // Redirect to the Articles page after successful deletion
                return RedirectToAction("Articles", "ContentHub");
            }
            catch (Exception ex)
            {

                // Show an error page or redirect back with an error message
                TempData["Error"] = "An error occurred while trying to delete the article.";
                return RedirectToAction("Articles", "ContentHub");
            }
        }

        #endregion

        #region ReleaseNotes

        [SessionAuthorize(["DEV"])]
        public async Task<IActionResult> ReleaseNotes()
        {
            try
            {
                var notes = await _dbAccess.GetReleaseNotesAsync();
                return View(await _dbAccess.GetReleaseNotesViewModelsAsync(notes));
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["DEV"])]
        [HttpGet("[controller]/ReleaseNotes/CreateReleaseNote")]
        public IActionResult CreateReleaseNotes()
        {
            try
            {
                ViewBag.Action = "action-create";
                return View("ReleaseNotesEditPage", new ReleaseNotes());
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["DEV"])]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/ReleaseNotes/CreateReleaseNote")]
        public async Task<IActionResult> CreateReleaseNotes([FromForm] ReleaseNotes newNote)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.CreateReleaseNoteAsync(newNote);
                return Ok(newNote.ReleaseNoteId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["DEV"])]
        [HttpGet("[controller]/ReleaseNotes/{id}")]
        public async Task<IActionResult> UpdateReleaseNote(int id)
        {
            try
            {
                ViewBag.Action = "action-update";
                var note = await _dbAccess.GetReleaseNoteAsync(id);
                if (note.ReleaseVersion != null && note.ReleaseVersion.Contains("("))
                    note.ReleaseVersion = note.ReleaseVersion?.Substring(0, note.ReleaseVersion.IndexOf("(")-1);
                return View("ReleaseNotesEditPage", note);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["DEV"])]
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("[controller]/ReleaseNotes/UpdateReleaseNote")]
        public async Task<IActionResult> UpdateReleaseNote(ReleaseNotes note)
        {
            note.ReleaseVersion = note.ReleaseVersion?.Trim();
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-update";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.UpdateReleaseNoteAsync(note);
                return Ok(note.ReleaseNoteId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["DEV"])]
        [HttpDelete("[controller]/ReleaseNotes/DeleteReleaseNote/{id}")]
        public async Task<IActionResult> DeleteReleaseNote(int id)
        {
            try
            {
                await _dbAccess.DeleteReleaseNoteAsync(id);
                return Ok(new { redirectToUrl = Url.Action("ReleaseNotes") }); // Since it's being called with ajax
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region AboutUs

        #region Mission

        [SessionAuthorize(["EXE"])]
        public async Task<IActionResult> Mission()
        {
            try
            {
                var mission = await _dbAccess.GetMissionAsync();
                return View(mission);
            } catch
            {
                return StatusCode(500);
            }
        }
        
        [HttpGet]
        [SessionAuthorize(["EXE"])]
        public async Task<IActionResult> UpdateMission(int id)
        {
            try
            {
                // Retrieve the mission by its ID
                var mission = await _dbAccess.GetMissionByIdAsync(id);

                // If the mission is not found, return a 404 view or redirect
                if (mission == null)
                {
                    return NotFound();
                }

                // Return the view with the mission model
                return View(mission);
            }
            catch
            {
                // Handle unexpected errors (e.g., log and return an error page)
                return StatusCode(500);
            }
        }


        [SessionAuthorize(["EXE"])]
        [HttpPut("[controller]/UpdateMission")]
        public async Task<IActionResult> UpdateMission([FromBody] Mission mission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.UpdateMissionAsync(mission);
                return Ok(mission);
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region Executives
        
        [SessionAuthorize(["EXE"])]
        public async Task<IActionResult> Executives()
        {
            try
            {
                var executives = await _dbAccess.GetExecutivesViewModelAsync();
                return View(executives);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/Executives/CreateExecutive")]
        public IActionResult CreateExecutive()
        {
            try
            {
                ViewBag.Action = "action-create";
                return View("ExecutiveEditPage", new Executive());
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/UpdateExecutive/{id}")]
        public async Task<IActionResult> UpdateExecutive(int id)
        {
            try
            {
                ViewBag.Action = "action-update";
                var executive = await _dbAccess.GetExecutiveAsync(id);
                return View("ExecutiveEditPage", executive);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost("[controller]/Executives/CreateExecutive")]
        public async Task<IActionResult> CreateExecutive([FromForm] ExecutiveDTO executive) {
            if (!ModelState.IsValid) {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }
                //AuthorizeResponse authorizeResponse = await _loginClient.AuthorizeAsync(username, _optionsProvider.AppCode);
                //UserBLL amsUser = authorizeResponse.Body.AuthorizeResult;

                try {
                    var newExecutive = new Executive {
                        FirstName = executive.FirstName,
                        LastName = executive.LastName,
                        Email = executive.Email,
                        Description = executive.Description,
                    };
                    var positions = executive.Positions?.Split(",");
                    newExecutive.ExecutivePositions = new List<ExecutivePosition>();
                    if (positions != null) {
                        foreach (var positionName in positions!) {
                            var trimmedPositionName = positionName.Trim();
                            var positionList = await _dbAccess.GetPositionsAsync();
                            var existingPosition = positionList.FirstOrDefault(p => p.PositionName == trimmedPositionName);


                            var position = existingPosition ?? new Position { PositionName = trimmedPositionName };

                            newExecutive.ExecutivePositions.Add(new ExecutivePosition {
                                ExecutiveId = executive.ExecutiveId,
                                Executive = newExecutive,
                                Position = position
                            });
                        }
                    }
                await _dbAccess.CreateExecutiveAsync(newExecutive);
                return Ok(executive.ExecutiveId);
            } catch {
                    return StatusCode(500);
                }
            }
        
        [SessionAuthorize(["EXE"])]
        [HttpPut("[controller]/Executives/UpdateExecutive")]
        public async Task<IActionResult> UpdateExecutive([FromForm] ExecutiveDTO executive) {
            try {
                var newExecutive = new Executive {
                    ExecutiveId = executive.ExecutiveId,
                    FirstName = executive.FirstName,
                    LastName = executive.LastName,
                    Email = executive.Email,
                    Description = executive.Description,
                };
                var positions = executive.Positions?
                    .Split(',')
                    .Where(position => !string.IsNullOrWhiteSpace(position))
                    .ToList();
                newExecutive.ExecutivePositions = new List<ExecutivePosition>();
                foreach (var positionName in positions!) {
                    var trimmedPositionName = positionName.Trim();
                    var positionList = await _dbAccess.GetPositionsAsync();
                    var existingPosition = positionList.FirstOrDefault(p => p.PositionName == trimmedPositionName);


                    var position = existingPosition ?? new Position { PositionName = trimmedPositionName };

                    newExecutive.ExecutivePositions.Add(new ExecutivePosition {
                        ExecutiveId = executive.ExecutiveId,
                        Executive = newExecutive,
                        Position = position
                    });
                }

                await _dbAccess.UpdateExecutiveAsync(newExecutive);
                return Ok(executive);
            }
            catch {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpDelete("[controller]/Executives/DeleteExecutive/{id}")]
        public async Task<IActionResult> DeleteExecutive(int id)
        {
            try
            {
                await _dbAccess.DeleteExecutiveAsync(id);
                return Ok(new { redirectToUrl = Url.Action("Executives") });
            } catch
            {
                return StatusCode(500);
            }
        }

        #endregion

        #region Elected Members

        [SessionAuthorize(["EXE"])]
        public async Task<IActionResult> ElectedMembers()
        {
            try
            {
                List<ElectedGroupTableViewModel> members = await _dbAccess.GetElectedMembersAsync();
                return View(members);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/ElectedMembers/CreateElectedMembers")]
        public IActionResult CreateElectedMember()
        {
            try
            {
                ViewBag.Action = "action-create";
                return View("ElectedMembersEditPage", new ElectedGroup());
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpGet("[controller]/ElectedMembers/{id}")]
        public async Task<IActionResult> UpdateElectedMember(int id)
        {
            try
            {
                ViewBag.Action = "action-update";
                var group = await _dbAccess.GetElectedGroupAsync(id);
                return View("ElectedMembersEditPage", group);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost("[controller]/ElectedMembers/CreateElectedMembers")]
        public async Task<IActionResult> CreateElectedMembers([FromForm] ElectedGroup electedGroup)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-create";
                return BadRequest(ModelState);
            }
            try
            {
                var id = await _dbAccess.CreateElectedGroupAsync(electedGroup);
                return Ok(id);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpPut("[controller]/ElectedMembers/UpdateElectedMembers")]
        public async Task<IActionResult> UpdateElectedMembers([FromForm] ElectedGroup electedGroup)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "action-update";
                return BadRequest(ModelState);
            }
            try
            {
                await _dbAccess.UpdateElectedGroupAsync(electedGroup);
                return Ok(electedGroup.GroupId);
            } catch
            {
                return StatusCode(500);
            }
        }

        [SessionAuthorize(["EXE"])]
        [HttpPost("[controller]/DeleteElectedMembers")]
        public async Task<IActionResult> DeleteElectedMembers(int id)
        {
            try
            {
                await _dbAccess.DeleteElectedGroupAsync(id);
                return RedirectToAction("ElectedMembers"); // Redirect to the same page after deletion
            }
            catch
            {
                return StatusCode(500); // Return error if deletion fails
            }
        }
        
        #endregion

        #region Image

        [HttpPost]
        [Route("[controller]/Images/CreateImage")]
        public async Task<IActionResult> CreateImageAsync(IFormFile file, string imageEditData)
        {
            try
            {
                Image? image = await _imageService.CreateImageAsync(file, imageEditData);
                if (image == null)
                {
                    return StatusCode(500);
                }

                return Ok(image);

            } catch(Exception ex)
            {
                return StatusCode(500, new { Detail = ex.Message });
            }
        }

        [HttpGet]
        [Route("[controller]/Images/GetImage")]
        public async Task<IActionResult> GetImageAsync(int imageId)
        {
            try
            {
                Console.WriteLine(imageId);
                Image? image = await _dbAccess.GetImageAsync(imageId);
                if (image == null)
                {
                    return StatusCode(404);
                }

                return Ok(image);
            } catch
            {
                return StatusCode(500);
            }
        }
        #endregion

        #endregion
        
        #region ExcelFile

        [HttpGet]
        public IActionResult UploadExcelFile() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelFile(IFormFile? excelFile) {
            if (excelFile == null || excelFile.Length == 0) {
                ModelState.AddModelError(String.Empty, "Please select a file");
            }

            if (!ModelState.IsValid) {
                return View();
            }

            if (!excelFile.FileName.EndsWith(".xlsx")) {
                ModelState.AddModelError(String.Empty, "Please upload an Excel (.xlsx) file");
            }

            if (!ModelState.IsValid) {
                return View();
            }


            using (var stream = excelFile.OpenReadStream()) {
                var result = ExcelJsonETL.ProcessExcelFile(stream);

                if (!result.Success)
                    return BadRequest(result.Errors);

                var departments = JsonConvert.DeserializeObject<List<ExcelJsonETL.DepartmentGroup>>(result.Json);

                //---------- Executives ----------
                var executiveUsers = departments
                    .Where(d => d.DepartmentName.Contains("executive", StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(d => d.Roles
                        .SelectMany(r => r.Users
                            .Select(user => new {
                                UserName = user,
                                RoleName = r.RoleName
                            })
                        )
                    )
                    .GroupBy(u => u.UserName)
                    .Select(g => new UserWithRoles {
                        UserName = g.Key,
                        Roles = g.Select(x => x.RoleName).OrderBy(name => name).ToList()
                    })
                    .OrderBy(u => u.UserName)
                    .ToList();

                var currentExecutiveUsersList = await _dbAccess.GetExecutivesAsync();
                var currentExecutiveUsers = currentExecutiveUsersList.Select(e => new UserWithRoles {
                    UserName = $"{e.FirstName} {e.LastName}",
                    Roles = e.ExecutivePositions
                            .Select(ep => ep.Position.PositionName)
                            .OrderBy(name => name)
                            .ToList()
                })
                    .OrderBy(u => u.UserName)
                    .ToList();

                var addedExecs = executiveUsers
                    .Except(currentExecutiveUsers, new UserWithRolesComparer())
                    .ToList();

                var removedExecs = currentExecutiveUsers
                    .Except(executiveUsers, new UserWithRolesComparer())
                    .ToList();

                var unchangedExecs = executiveUsers
                    .Intersect(currentExecutiveUsers, new UserWithRolesComparer())
                    .ToList();

                //-------------------------------
                //----------- Members -----------

                var memberUsers = departments
                    .Where(d => !d.DepartmentName.Contains("executive", StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(d => d.Roles
                        .Select(r => new {
                            RoleName = r.RoleName,
                            Users = r.Users
                        })
                    )
                    .GroupBy(r => r.RoleName)
                    .Select(g => new RoleWithUsers {
                        RoleName = g.Key,
                        Users = g.SelectMany(x => x.Users).Distinct().OrderBy(user => user).ToList()
                    })
                    .OrderBy(r => r.RoleName)
                    .ToList();

                var currentMemberUsersList = await _dbAccess.GetElectedMembersAsync();
                var currentMemberUsers = currentMemberUsersList
                    .Select(m => new RoleWithUsers {
                        RoleName = m.ElectedGroupName,
                        Users = m.ElectedMembers.Select(x => x.MemberFirstName + " " + x.MemberLastName).OrderBy(x => x).ToList()
                    })
                    .OrderBy(r => r.RoleName)
                    .ToList();

                var addedMembers = memberUsers
                    .Except(currentMemberUsers, new RoleWithUsersComparer())
                    .ToList();

                var removedMembers = currentMemberUsers
                    .Except(memberUsers, new RoleWithUsersComparer())
                    .ToList();

                var unchangedMembers = memberUsers
                    .Intersect(currentMemberUsers, new RoleWithUsersComparer())
                    .ToList();

                //-------------------------------

                var execModel = new { AddList = addedExecs, RemoveList = removedExecs, UnchangedList = unchangedExecs };

                var memberModel = new { AddList = addedMembers, RemoveList = removedMembers, UnchangedList = unchangedMembers };

                var model = new { ExecModel = execModel, MemberModel = memberModel };

                return View(model as dynamic);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmExcelChanges([FromBody] UserList userList) {
            if (userList == null) {
                return BadRequest("No data provided");
            }
            
            //---------- Executives ----------
            var executives = await _dbAccess.GetExecutivesAsync();
            
            foreach (var user in userList.ExecRemoveList) {
                var exec = executives.FirstOrDefault(e => String.Equals(e.FirstName + " " + e.LastName, user.UserName, StringComparison.InvariantCultureIgnoreCase));
                if (exec != null) {
                    await DeleteExecutive(exec.ExecutiveId);
                }
            }

            foreach (var user in userList.ExecAddList) {
                var firstName = user.UserName.Split(" ")[0];
                var lastName = String.Join(" ", user.UserName.Split(" ").Skip(1));
                var execEmail = firstName.Substring(0, 1) + lastName + "@cegep-heritage.qc.ca";
                var execDTO = new ExecutiveDTO {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = execEmail,
                    Positions = user.Roles == null ? "" : String.Join(",", user.Roles),
                };
                await CreateExecutive(execDTO);
            }
            //-------------------------------
            //----------- Members -----------
            var member = await _dbAccess.GetElectedMembersAsync();

            foreach (var role in userList.MemberRemoveList) {
                var memberGroup = member.FirstOrDefault(m => m.ElectedGroupName == role.RoleName);
                if (memberGroup != null) {
                    await DeleteElectedMembers(memberGroup.ElectedGroupId);
                }
            }
            
            foreach (var role in userList.MemberAddList) {
                var electedGroup = new ElectedGroup {
                    GroupName = role.RoleName
                };
                var groupId = await _dbAccess.CreateElectedGroupAsync(electedGroup);
                var electedMembers = new List<ElectedMember>();
                foreach (var memName in role.Users) {
                    var firstName = memName.Split(" ")[0];
                    var lastName = String.Join(" ", memName.Split(" ").Skip(1));
                    var electedMember = new ElectedMember {
                        GroupId = groupId,
                        MemberFirstName = firstName,
                        MemberLastName = lastName
                    };
                    electedMembers.Add(electedMember);
                }
                electedGroup.GroupId = groupId;
                electedGroup.Members = electedMembers;
                await _dbAccess.UpdateElectedGroupAsync(electedGroup);
            }
            //-------------------------------
            
            return Ok();
        }

        #endregion
    }

    public class UserList {
        public List<UserWithRoles> ExecAddList { get; set; }
        public List<UserWithRoles> ExecRemoveList { get; set; }
        
        public List<RoleWithUsers> MemberAddList { get; set; }
        public List<RoleWithUsers> MemberRemoveList { get; set; }
    }

    public class UserWithRoles {
        public string UserName { get; set; }
        public List<string>? Roles { get; set; }
    }
    
    public class RoleWithUsers {
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
    
    public class UserWithRolesComparer : IEqualityComparer<UserWithRoles>
    {
        public bool Equals(UserWithRoles x, UserWithRoles y)
        {
            if (x == null || y == null)
                return false;

            return x.UserName == y.UserName &&
                   x.Roles.SequenceEqual(y.Roles);
        }

        public int GetHashCode(UserWithRoles obj)
        {
            if (obj == null)
                return 0;

            int hashUserName = obj.UserName?.GetHashCode() ?? 0;
            int hashRoles = string.Join(",", obj.Roles).GetHashCode();

            return hashUserName ^ hashRoles;
        }
    }
    
    public class RoleWithUsersComparer : IEqualityComparer<RoleWithUsers>
    {
        public bool Equals(RoleWithUsers x, RoleWithUsers y)
        {
            if (x == null || y == null)
                return false;

            return x.RoleName == y.RoleName &&
                   x.Users.SequenceEqual(y.Users);
        }

        public int GetHashCode(RoleWithUsers obj)
        {
            if (obj == null)
                return 0;

            int hashRoleName = obj.RoleName?.GetHashCode() ?? 0;
            int hashUsers = string.Join(",", obj.Users).GetHashCode();

            return hashRoleName ^ hashUsers;
        }
    }
}
