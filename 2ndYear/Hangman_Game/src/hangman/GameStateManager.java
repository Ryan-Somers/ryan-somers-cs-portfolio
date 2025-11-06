package hangman;

import java.io.*;
import java.util.Arrays;

public class GameStateManager {

	public static void saveState(Game game, Scoreboard scoreboard, String currentPlayerName, int currentImageIndex) {
		Player currentPlayer = scoreboard.getPlayerByName(currentPlayerName);
		if (currentPlayer != null) {
			currentPlayer.setGameState(game);
			game.setCurrentImageIndex(currentImageIndex); // Save the current image index
		}
		try {
			FileOutputStream fileOut = new FileOutputStream("gameState.ser");
			ObjectOutputStream out = new ObjectOutputStream(fileOut);
			out.writeObject(scoreboard); // Save the entire scoreboard
			out.close();
			fileOut.close();
		} catch (IOException i) {
			i.printStackTrace();
		}
	} // saveState(Game, Scoreboard, String, int)

	public static Scoreboard loadState() {
		try (ObjectInputStream ois = new ObjectInputStream(new FileInputStream("gameState.ser"))) {
			return (Scoreboard) ois.readObject();
		} catch (FileNotFoundException e) {

		} catch (EOFException e) {

		} catch (ClassCastException e) {
			System.err.println("The saved data format has changed. Please delete or update the gameState.ser file.");
			e.printStackTrace();
		} catch (IOException | ClassNotFoundException e) {
			e.printStackTrace();
		}
		return new Scoreboard();
	} // loadState()

} // GameStateManager