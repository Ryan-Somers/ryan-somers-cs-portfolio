package hangman;

import javax.swing.*;
import java.awt.*;

public class ScoreboardDialog extends JDialog {
	private Scoreboard scoreboard;

	public ScoreboardDialog(JFrame parent, Scoreboard scoreboard) {
		super(parent, "Scoreboard", true);
		this.scoreboard = scoreboard;

		setSize(400, 300);
		setLocationRelativeTo(parent);
		setLayout(new BorderLayout());

		String[] columnNames = { "Player Name", "Games Played", "Wins" };
		Object[][] data = new Object[scoreboard.getNumberOfPlayers()][3];

		for (int i = 0; i < scoreboard.getNumberOfPlayers(); i++) {
			Player player = scoreboard.getPlayer(i);
			data[i][0] = player.getName();
			data[i][1] = player.getNumberGamesPlayed();
			data[i][2] = player.getNumberGamesWon();
		} // for()

		JTable table = new JTable(data, columnNames);
		JScrollPane scrollPane = new JScrollPane(table);
		add(scrollPane, BorderLayout.CENTER);
		for (int i = 0; i < scoreboard.getNumberOfPlayers(); i++) {
			System.out.println(scoreboard.getPlayer(i).getName());
		} // for()

	} // ScoreboardDialog(JFrame, Scoreboard)
} // ScoreboardDialog