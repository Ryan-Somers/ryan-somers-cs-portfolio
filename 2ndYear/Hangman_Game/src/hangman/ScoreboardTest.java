package hangman;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

public class ScoreboardTest {

	private Scoreboard scoreboard;

	@BeforeEach
	public void setUp() {
		scoreboard = new Scoreboard();
	} // setUp()

	@Test
	public void testDefaultConstructor() {
		assertEquals(0, scoreboard.getNumberOfPlayers());
	} // testDefaultConstructor()

	@Test
	public void testAddPlayer() {
		scoreboard.addPlayer(new Player("John"));
		assertEquals(1, scoreboard.getNumberOfPlayers());
		assertNotNull(scoreboard.getPlayerByName("John"));
	} // testAddPlayer()

	@Test
	public void testAddMultiplePlayers() {
		scoreboard.addPlayer(new Player("John"));
		scoreboard.addPlayer(new Player("Alice"));
		scoreboard.addPlayer(new Player("Bob"));
		assertEquals(3, scoreboard.getNumberOfPlayers());
	} // testAddMultiplePlayers()

	@Test
	public void testGamePlayedWin() {
		scoreboard.gamePlayed("John", true);
		Player player = scoreboard.getPlayerByName("John");
		assertEquals(1, player.getNumberGamesPlayed());
		assertEquals(1, player.getNumberGamesWon());
	} // testGamePlayedWin()

	@Test
	public void testGamePlayedLoss() {
		scoreboard.gamePlayed("John", false);
		Player player = scoreboard.getPlayerByName("John");
		assertEquals(1, player.getNumberGamesPlayed());
		assertEquals(0, player.getNumberGamesWon());
	} // testGamePlayedLoss()

	@Test
	public void testGamePlayedPlayerNotFound() {
		scoreboard.gamePlayed("NonExistent", true);
		Player player = scoreboard.getPlayerByName("NonExistent");
		assertNotNull(player);
		assertEquals(1, player.getNumberGamesPlayed());
		assertEquals(1, player.getNumberGamesWon());
	} // testGamePlayedPlayerNotFound()

	@Test
	public void testGetNextPlayerEmpty() {
		assertThrows(IndexOutOfBoundsException.class, () -> {
			scoreboard.getPlayer(0);
		});
	} // testGetNextPlayerEmpty()

	@Test
	public void testGetNextPlayerListOfSizeOne() {
		scoreboard.addPlayer(new Player("John"));
		assertNotNull(scoreboard.getPlayer(0));
	} // testGetNextPlayerListOfSizeOne()

	@Test
	public void testGetNextPlayerListOfSizeThree() {
		scoreboard.addPlayer(new Player("John"));
		scoreboard.addPlayer(new Player("Alice"));
		scoreboard.addPlayer(new Player("Bob"));
		assertNotNull(scoreboard.getPlayer(2));
	} // testGetNextPlayerListOfSizeThree()
} // ScoreboardTest
