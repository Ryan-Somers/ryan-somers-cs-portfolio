package hangman;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

public class PlayerTest {

	private Player player;

	@BeforeEach
	public void setUp() {
		player = new Player("John");
	} // setUp()

	@Test
	public void testConstructor() {
		assertEquals("John", player.getName());
		assertEquals(0, player.getNumberGamesPlayed());
		assertEquals(0, player.getNumberGamesWon());
	} // testConstructor()

	@Test
	public void testSets() {
		player.incrementGamesPlayed();
		player.incrementGamesWon();
		assertEquals(1, player.getNumberGamesPlayed());
		assertEquals(1, player.getNumberGamesWon());
	} // testSets()
} // PlayerTest
