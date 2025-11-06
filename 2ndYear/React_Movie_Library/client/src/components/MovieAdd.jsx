import React, { useState } from 'react';
import { Toaster, toast } from 'sonner';

const MovieAdd = ({onBackToList}) => {
  const [movie, setMovie] = useState({
    title: '',
    year: '',
    runtime: '',
    revenue: '',
    actors: '',
    genres: '' 
  });

  const handleChange = (e) => {
    setMovie({ ...movie, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const movieData = {
      Title: movie.title,
      Actors: movie.actors.split(',').map(actor => actor.trim()),
      Genre: movie.genres.split(',').map(genre => genre.trim()), 
      Year: movie.year ? Number(movie.year) : null,
      Runtime: movie.runtime ? Number(movie.runtime) : null,
      Revenue: movie.revenue ? Number(movie.revenue) : null
    };

    try {
      const response = await fetch('/movies', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(movieData)
      });

      if (response.ok) {
        console.log('Movie added successfully');
        setMovie({ title: '', year: '', runtime: '', revenue: '', actors: '', genres: '' });
        toast.success('Movie added successfully');
      } else {
        toast.error('Failed to add movie');
        throw new Error('Failed to add movie');
      }
    } catch (error) {
      console.error('Error adding movie:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col items-center justify-center max-w-md p-4 mx-auto my-8 bg-white rounded shadow-lg">
  <h2 className="mb-4 text-2xl font-semibold">Add Movie</h2>
  
  <div className="w-full mb-4">
    <input required type="text" name="title" placeholder="Title" value={movie.title} onChange={handleChange}
           className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"/>
  </div>

  <div className="w-full mb-4">
    <input required type="number" name="year" placeholder="Year" value={movie.year} onChange={handleChange}
           className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"/>
  </div>

  <div className="w-full mb-4">
    <input required type="text" name="runtime" placeholder="Runtime" value={movie.runtime} onChange={handleChange}
           className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"/>
  </div>

  <div className="w-full mb-4">
    <input required type="text" name="revenue" placeholder="Revenue" value={movie.revenue} onChange={handleChange}
           className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"/>
  </div>

  <div className="w-full mb-4">
    <input required type="text" name="actors" placeholder="Actors (comma separated)" value={movie.actors} onChange={handleChange}
           className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"/>
  </div>

  <div className="w-full mb-6">
    <input required type="text" name="genres" placeholder="Genres (comma separated)" value={movie.genres} onChange={handleChange}
           className="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"/>
  </div>

  <div className="flex justify-between w-full mt-4">
        <button type="submit" className="px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-600">
          Add Movie
        </button>
        <button type="button" onClick={onBackToList} className="px-4 py-2 text-white bg-gray-500 rounded hover:bg-gray-600">
          Back to Movies
        </button>
      </div>
  
</form>

  );
};

export default MovieAdd;
