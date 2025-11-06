// YearSelect.jsx
import React, { useState } from 'react';

const YearSelect = ({ onYearMoviesFetched }) => {
  const [year, setYear] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(`/years/${year}`);
      if (!response.ok) {
        throw new Error(`Failed to fetch movies for year ${year}. Status: ${response.status}`);
      }
      const movies = await response.json();
      onYearMoviesFetched(movies); 
    } catch (error) {
      console.error('Error fetching movies:', error);
    }
  };

  return (
    <div className='px-4 my-4'>
      <form onSubmit={handleSubmit}>
        <label htmlFor="year" className='block mb-2 text-sm font-medium text-gray-700'>
          Find Movies with Year:
        </label>
        <div className='grid grid-cols-1 gap-2 sm:grid-cols-8'>
          <input
            id="year"
            type="number"
            className='px-2 py-1 border rounded-md' 
            value={year}
            onChange={(e) => setYear(e.target.value)}
          />
          <button 
            type="submit" 
            className='px-2 py-1 mt-2 text-white bg-blue-500 rounded-md hover:bg-blue-600 sm:mt-0' 
          >
            Fetch Movies
          </button>
        </div>
      </form>
    </div>
  );
};
export default YearSelect;
