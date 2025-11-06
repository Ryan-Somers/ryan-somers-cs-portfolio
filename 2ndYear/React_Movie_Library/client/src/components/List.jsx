// List.jsx
import React, { useState, useEffect } from 'react';

function List({ onMovieSelect, actorMovies, setShowAddMovieForm}) {
  const [movies, setMovies] = useState([]);

  // Standalone function to fetch all movies
  const fetchMovies = async () => {
    try {
      const response = await fetch('/movies');
      if (!response.ok) {
        throw new Error('Failed to fetch movies');
      }
      const data = await response.json();
      setMovies(data);
    } catch (error) {
      console.error('Error:', error);
    }
  };

  // Fetch all movies initially
  useEffect(() => {
    fetchMovies();
  }, []);

  // Update movies based on actor selection, if provided
  useEffect(() => {
    if (actorMovies) {
      setMovies(actorMovies);
    }
  }, [actorMovies]);

  // Handle click on reset button
  const handleResetClick = () => {
    fetchMovies(); // Refetch all movies
  };

  if (!movies.length) {
    return <h1 className='text-center'>Could Not Find Movies</h1>;
  }

  // Function to show the add movie form
  const handleShowAddMovieForm = () => {
    setShowAddMovieForm(true);
  };

  return (
    <main>
     <div className='grid grid-cols-1 mb-4 text-center md:grid-cols-8'>
    <button
      onClick={handleResetClick}
      className="px-4 py-2 mb-2 text-white bg-blue-500 rounded-md md:col-span-1 md:col-start-4 md:mb-0 md:mr-2 hover:bg-blue-600"
    >
      Reset to All Movies
    </button>
    <button
      onClick={handleShowAddMovieForm}
      className="px-4 py-2 text-white bg-green-500 rounded-md md:col-span-1 md:col-start-5 hover:bg-green-600"
    >
      Add New Movie
    </button>
  </div>
  <div className='grid grid-cols-1 gap-4 p-4 md:grid-cols-3'>
        {movies.map((movie, index) => (
          <div key={index} className='p-2 text-center border rounded-md bg-slate-300'>
            <h3 className='text-2xl'>{movie.title}</h3>
            <p>{movie.year}</p>
            <button
              className="px-2 py-1 mt-2 text-white bg-blue-500 rounded-md"
              onClick={() => onMovieSelect(movie)}
            >
              Detailed Info
            </button>
          </div>
        ))}
      </div>
    </main>
  );
}

export default List;
