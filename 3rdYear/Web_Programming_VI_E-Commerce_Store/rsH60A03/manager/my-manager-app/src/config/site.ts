export type SiteConfig = typeof siteConfig;

export const siteConfig = {
  name: "Ryan's Store",
  description: "Manage your amazing items with ease.",
  navItems: [
    {
      label: "Home",
      href: "/",
    },
    {
      label: "Products",
      href: "/products",
    },
    {
      label: "Reports",
      href: "/reports",
    },
  ],
  navMenuItems: [
    {
      label: "Home",
      href: "/",
    },
    {
      label: "Products",
      href: "/products",
    },
    {
      label: "Reports",
      href: "/reports",
    },
  ],
  links: {
    github: "https://github.com/HeritageCollegeClassroom/420-h60-hr-assignment-3-Ryan-Somers",
    twitter: "https://twitter.com/ryansomers12",
    docs: "/products",
    reports: "/reports",
  },
};
