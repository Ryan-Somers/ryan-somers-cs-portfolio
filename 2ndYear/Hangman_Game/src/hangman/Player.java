package hangman;

import java.io.Serializable;
import java.util.List;

public class Player implements Serializable {
	private static final long serialVersionUID = 1L;
	private String name;
	private int numberGamesPlayed;
	private int numberGamesWon;
	private Game gameState; // Store the game state for this player

	public Player(String name) {
		if (name == null || name.trim().isEmpty()) {
			throw new IllegalArgumentException("Player name cannot be null or empty");
		}
		this.name = name;
		this.numberGamesPlayed = 0;
		this.numberGamesWon = 0;
	} // Player(String)

	public Game getGameState() {
		return gameState;
	} // getGameState()

	public void setGameState(Game gameState) {
		this.gameState = gameState;
	} // setGameState(Game)

	public String getName() {
		return name;
	} // getName()

	public void setName(String name) {
		if (name == null || name.trim().isEmpty()) {
			throw new IllegalArgumentException("Player name cannot be null or empty");
		}
		this.name = name;
	} // setName(String)

	public int getNumberGamesPlayed() {
		return numberGamesPlayed;
	} // getNumberGamesPlayed()

	public void incrementGamesPlayed() {
		this.numberGamesPlayed++;
	} // incrementGamesPlayed()

	public int getNumberGamesWon() {
		return numberGamesWon;
	} // getNumberGamesWon()

	public void incrementGamesWon() {
		this.numberGamesWon++;
	} // incrementGamesWon()
}
