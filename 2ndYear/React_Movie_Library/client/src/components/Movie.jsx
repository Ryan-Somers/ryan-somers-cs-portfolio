import React, { useState, useEffect } from 'react';
import Actor from '../components/Actor';
import Genre from '../components/Genre';

function Movie({ selectedMovie, onBackToMain }) {
  const [movieDetails, setMovieDetails] = useState(null);

  useEffect(() => {
    if (selectedMovie && selectedMovie.id) { 
      const movieDetailsUrl = `/movies/${selectedMovie.id}`; 
      fetch(movieDetailsUrl)
        .then((response) => {
          if (!response.ok) {
            throw new Error('Failed to fetch movie details');
          }
          return response.json();
        })
        .then((data) => setMovieDetails(data))
        .catch((error) => console.error('Error fetching movie details:', error));
    }
  }, [selectedMovie]);

  if (!selectedMovie) {
    return <div>Select a movie for detailed info</div>;
  }

  if (!movieDetails) {
    return <div>Loading...</div>;
  }

  return (
    <main>
    <div className='grid h-screen place-items-center'>
      <div className='p-4 text-center bg-gray-300 rounded-md'>
        <h2 className='font-semibold text-center'>Movie Details</h2>
        <ul>
          <li>
            <p className=''>{movieDetails.Title}</p>
            <p>Released in: {movieDetails.Year}</p>
            <p>{movieDetails.Runtime} minutes</p>
            <p>Made ${movieDetails.Revenue}M dollars</p>
            <Actor actors={movieDetails.Actors} />
            <Genre genres={movieDetails.Genre} />
          </li>
        </ul>
        
      </div>
      <button onClick={onBackToMain} className="px-4 py-2 mt-4 text-white bg-blue-500 rounded hover:bg-blue-600">
          Back to Movies List
        </button>
    </div>
    </main>
  );
}

export default Movie;
