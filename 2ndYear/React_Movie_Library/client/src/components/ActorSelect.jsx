import React, { useState, useEffect } from 'react';

const ActorSelect = ({ onMoviesFetched }) => {
  const [actorInput, setActorInput] = useState('');

  const fetchMoviesByActor = async () => {
    // Fetch all movies if input is empty
    if (actorInput.trim() === '') {
      const response = await fetch('/movies');
      if (response.ok) {
        const movies = await response.json();
        onMoviesFetched(movies);
      } else {
        console.error('Failed to fetch all movies');
      }
    }
    // Fetch movies by actor if input is not empty
    else {
      const response = await fetch(`/actors/${actorInput}`);
      if (response.ok) {
        const movies = await response.json();
        onMoviesFetched(movies);
      } else {
        console.error('Failed to fetch movies for actor');
      }
    }
  };

  useEffect(() => {
    const delayDebounceFn = setTimeout(() => {
      fetchMoviesByActor();
    }, 300);

    return () => clearTimeout(delayDebounceFn);
  }, [actorInput]);

  const handleInputChange = (event) => {
    setActorInput(event.target.value);
  };

  return (
    <div className='px-4 my-4'> 
      <h5 className='p-5 text-3xl font-semibold'>Search:</h5>
      <label className='block mb-2 text-sm font-medium text-gray-700'> 
        Find Movies with Actor:
      </label>
      <div className='grid grid-cols-1 gap-2 sm:grid-cols-8'>
        <input
          type='text'
          className='px-2 py-1 border rounded-md'
          value={actorInput}
          onChange={handleInputChange}
        />
      </div>
    </div>
  );
};

export default ActorSelect;
