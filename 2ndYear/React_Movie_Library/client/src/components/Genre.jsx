// Genre.jsx
import React from 'react';

function Genre({ genres }) {
  if (!genres || !genres.length) {
    return <div>No genres available</div>;
  }

  return (
    <div>
      <h3 className='font-semibold'>Genres:</h3>
      <ul>
        {genres.map((genre, index) => (
          <li key={index}>{genre}</li>
        ))}
      </ul>
    </div>
  );
}

export default Genre;