import React, { useState } from 'react';
import List from '../components/List';
import Movie from '../components/Movie';
import ActorSelect from '../components/ActorSelect';
import YearSelect from '../components/YearSelect';
import MovieAdd from '../components/MovieAdd';
import { Toaster, toast } from 'sonner';

function App() {
  const [selectedMovie, setSelectedMovie] = useState(null);
  const [movies, setMovies] = useState([]);
  const [showAddMovieForm, setShowAddMovieForm] = useState(false);
  const [showSearchInputs, setShowSearchInputs] = useState(true);

  const handleMovieSelect = (movie) => {
    const movieIdentifier = movie.id || movie.key;
    setSelectedMovie({ ...movie, id: movieIdentifier });
    setShowSearchInputs(false);
  };

  const handleBackToList = () => {
    setShowAddMovieForm(false);
  };

  const handleMoviesFetched = (fetchedMovies) => {
    setMovies(fetchedMovies);
  };

  const handleYearMoviesFetched = (movies) => {
    setMovies(movies);
  };const handleBackToMain = () => {
    setSelectedMovie(null); // Resets the selected movie
    setShowSearchInputs(true)
  };

  return (
    <>
    <main className='bg-white'>
     <Toaster richColors/>
      <h1 className='p-4 text-3xl font-semibold text-center'>Movies List</h1>
      
      {showAddMovieForm ? (
        <MovieAdd onBackToList={handleBackToList}/>
      ) : (
        <>
          {showSearchInputs && (
        <>
          <ActorSelect onMoviesFetched={handleMoviesFetched} />
          <YearSelect onYearMoviesFetched={handleYearMoviesFetched} />
        </>
      )}
          
          {selectedMovie ? (
            <Movie selectedMovie={selectedMovie}onBackToMain={handleBackToMain} />
          ) : (
            <List onMovieSelect={handleMovieSelect} actorMovies={movies} setShowAddMovieForm={setShowAddMovieForm} />
          )}
        </>
      )}
      </main>
    </>
  );
}

export default App;
