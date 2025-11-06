const express = require("express");
const path = require("path");
const fs = require("fs");

const app = express();
const port = 8880;

app.use(express.static(path.join(__dirname, "client", "public")));

app.get("/movies", (req, res) => {
  fs.readFile(path.join(__dirname, "data", "movies.json"), "utf8", (err, data) => {
    if (err) {
      console.error(err);
      res.status(500).send("Internal Server Error");
    } else {
      const movies = JSON.parse(data);
      const sortedMovies = movies.sort((a, b) => a.Title.localeCompare(b.Title));
      const movieList = sortedMovies.map((movie) => ({
        key: movie.Key,
        title: movie.Title,
        year: movie.Year,
      }));
      res.json(movieList);
    }
  });
});

app.get("/movies/:id", (req, res) => {
  const movieId = Number(req.params.id);
  fs.readFile(path.join(__dirname, "data", "movies.json"), "utf8", (err, data) => {
    if (err) {
      console.error(err);
      res.status(500).send("Internal Server Error");
    } else {
      const movies = JSON.parse(data);
      const movie = movies.find((movie) => movie.Key === movieId);
      if (movie) {
        res.json(movie);
      } else {
        res.status(404).send("Movie not found");
      }
    }
  });
});

app.get("/actors/:name", (req, res) => {
  const actorName = req.params.name;
  fs.readFile(path.join(__dirname, "data", "movies.json"), "utf8", (err, data) => {
    if (err) {
      console.error(err);
      res.status(500).send("Internal Server Error");
    } else {
      const movies = JSON.parse(data);

      // Use a case-insensitive regular expression to match actors starting with the provided name
      const actorRegex = new RegExp(`^${actorName}`, "i");

      const actorMovies = movies.filter((movie) => {
        // Use some() to check if at least one actor matches the regular expression
        return movie.Actors.some((actor) => actor.match(actorRegex));
      });

      const sortedMovies = actorMovies.sort((a, b) => a.Title.localeCompare(b.Title));
      const movieList = sortedMovies.map((movie) => ({
        key: movie.Key,
        title: movie.Title,
        year: movie.Year,
      }));

      res.json(movieList);
    }
  });
});

app.get("/years/:year", (req, res) => {
  const requestedYear = Number(req.params.year);
  fs.readFile(path.join(__dirname, "data", "movies.json"), "utf8", (err, data) => {
    if (err) {
      console.error(err);
      res.status(500).send("Internal Server Error");
    } else {
      const movies = JSON.parse(data);
      const moviesOfYear = movies.filter((movie) => movie.Year === requestedYear);
      const sortedMovies = moviesOfYear.sort((a, b) => a.Title.localeCompare(b.Title));
      const movieList = sortedMovies.map((movie) => ({
        id: movie.Key,
        title: movie.Title,
      }));
      res.json(movieList);
    }
  });
});

app.use(express.json());

app.post("/movies", (req, res) => {
  fs.readFile(path.join(__dirname, "data", "movies.json"), "utf8", (err, data) => {
    if (err) {
      console.error(err);
      return res.status(500).send("Internal Server Error");
    }

    // Parse existing movies and find the maximum key
    const movies = JSON.parse(data);
    const maxKey = movies.reduce((max, movie) => Math.max(max, movie.Key), 0);

    // New movie object with incremented key
    const newMovie = { ...req.body, Key: maxKey + 1 };

    // Add the new movie to the array
    movies.push(newMovie);

    // Write the updated movies array back to the file
    fs.writeFile(path.join(__dirname, "data", "movies.json"), JSON.stringify(movies, null, 2), "utf8", (writeErr) => {
      if (writeErr) {
        console.error(writeErr);
        return res.status(500).send("Failed to save the movie");
      }
      res.send("Movie added successfully");
    });
  });
});

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
