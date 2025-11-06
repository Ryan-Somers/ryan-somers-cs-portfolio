package hangman;

import java.io.Serializable;

import hangman.linked_data_structures.DoublyLinkedList;

public class Scoreboard implements Serializable {
	private DoublyLinkedList<Player> players;

	public Scoreboard() {

		this.players = new DoublyLinkedList<>();

	} // Scoreboard()

	public void addPlayer(Player player) {
		if (players.getLength() == 0) {
			players.add(player);
			return;
		}

		// Find the correct position to insert the new player
		int position = 0;
		while (position < players.getLength()) {
			if (player.getName() != null && players.getElementAt(position).getName() != null
					&& player.getName().compareToIgnoreCase(players.getElementAt(position).getName()) <= 0) {
				break;
			}
			position++;
		}
		// Insert the player at the found position
		players.add(player, position);
	} // addPlayer(Player)

	public Player getPlayerByName(String name) {
		for (int i = 0; i < players.getLength(); i++) {
			if (players.getElementAt(i).getName().equals(name)) {
				return players.getElementAt(i);
			}
		}
		return null;
	} // getPlayerByName(String)

	public boolean hasPlayer(String name) {
		return getPlayerByName(name) != null;
	} // hasPlayer(String)

	public void gamePlayed(String name, boolean win) {
		Player player = getPlayerByName(name);
		if (player == null) {
			player = new Player(name);
			addPlayer(player);
		}
		player.incrementGamesPlayed();
		if (win) {
			player.incrementGamesWon();
		}
	} // gamePlayed(String, boolean)

	public Player getPlayer(int index) {
		if (index < 0 || index >= players.getLength()) {
			throw new IndexOutOfBoundsException();
		}
		return players.getElementAt(index);
	} // getPlayer(int)

	public int getNumberOfPlayers() {
		return players.getLength();
	} // getNumberOfPlayers()
} // Scoreboard
