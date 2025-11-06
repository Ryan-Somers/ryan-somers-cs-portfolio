package HangmanLogic;

import android.content.Context;
import android.util.Log;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

public class GameStateManager {

	// Save the game state for a specific player
	public static void saveState(Context context, Game game, Scoreboard scoreboard, String currentPlayerName) {
		if (scoreboard != null && currentPlayerName != null && game != null) {
			Player currentPlayer = scoreboard.getPlayerByName(currentPlayerName);
			if (currentPlayer != null) {
				game.updateImageIndex(); // Update image index based on current game progress
				currentPlayer.setGameState(game);
			}
		}
		saveScoreboard(context, scoreboard);
		Log.d("GameStateManager", "Game state saved for player: " + currentPlayerName);
	}


	// Save the entire scoreboard (used for player management without game state)
	public static void saveScoreboard(Context context, Scoreboard scoreboard) {
		try {
			FileOutputStream fileOut = context.openFileOutput("gameState.ser", Context.MODE_PRIVATE);
			ObjectOutputStream out = new ObjectOutputStream(fileOut);
			out.writeObject(scoreboard);
			out.close();
			fileOut.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	// Load the scoreboard from storage
	public static Scoreboard loadState(Context context) {
		try {
			FileInputStream fileInputStream = context.openFileInput("gameState.ser");
			ObjectInputStream objectInputStream = new ObjectInputStream(fileInputStream);
			Scoreboard scoreboard = (Scoreboard) objectInputStream.readObject();
			objectInputStream.close();
			fileInputStream.close();
			return scoreboard;
		} catch (FileNotFoundException e) {
			// File not found, return a new scoreboard or null
			return new Scoreboard();
		} catch (IOException | ClassNotFoundException e) {
			e.printStackTrace();
			return null;
		}
	}



}
