function Actor({ actors }) {
  if (!actors || !actors.length) {
    return <div>No actors available</div>;
  }

  return (
    <div>
      <h3 className="font-semibold">Actors:</h3>
      <ul>
        {actors.map((actor, index) => (
          <li key={index}>{actor}</li>
        ))}
      </ul>
    </div>
  );
}

export default Actor;