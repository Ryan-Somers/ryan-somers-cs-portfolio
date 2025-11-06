package hangman;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;

public class LoginDialog extends JDialog {
	private JComboBox<String> playerNameComboBox;
	private JTextField newPlayerNameField;
	private JButton okButton;
	private Scoreboard scoreboard;
	private String selectedPlayerName;
	private CardLayout cardLayout;
	private JPanel cardPanel;

	public LoginDialog(JFrame parent, Scoreboard scoreboard) {
		super(parent, "Login", true);
		this.scoreboard = scoreboard;

		setLayout(new BorderLayout());

		// Radio buttons for new or returning player
		JRadioButton newPlayerButton = new JRadioButton("New Player");
		JRadioButton returningPlayerButton = new JRadioButton("Returning Player");
		ButtonGroup group = new ButtonGroup();
		group.add(newPlayerButton);
		group.add(returningPlayerButton);

		JPanel radioPanel = new JPanel();
		radioPanel.add(newPlayerButton);
		radioPanel.add(returningPlayerButton);
		add(radioPanel, BorderLayout.NORTH);

		// Card layout for name input or dropdown
		cardLayout = new CardLayout();
		cardPanel = new JPanel(cardLayout);

		newPlayerNameField = new JTextField(10);
		cardPanel.add(newPlayerNameField, "NewPlayer");

		playerNameComboBox = new JComboBox<>();
		for (int i = 0; i < scoreboard.getNumberOfPlayers(); i++) {
			playerNameComboBox.addItem(scoreboard.getPlayer(i).getName());
		} // for()
		cardPanel.add(playerNameComboBox, "ReturningPlayer");

		add(cardPanel, BorderLayout.CENTER);

		// Listener to switch card layout
		newPlayerButton.addActionListener(e -> cardLayout.show(cardPanel, "NewPlayer"));
		returningPlayerButton.addActionListener(e -> cardLayout.show(cardPanel, "ReturningPlayer"));

		okButton = new JButton("OK");
		okButton.addActionListener(e -> onOk());
		add(okButton, BorderLayout.SOUTH);

		// Add a Quit button
		JButton quitButton = new JButton("Quit");
		quitButton.addActionListener(e -> System.exit(0)); // Close the application

		JPanel buttonPanel = new JPanel();
		buttonPanel.add(okButton);
		buttonPanel.add(quitButton);
		add(buttonPanel, BorderLayout.SOUTH);

		// Listener to switch card layout and enable appropriate input
		newPlayerButton.addActionListener(e -> {
			cardLayout.show(cardPanel, "NewPlayer");
			newPlayerNameField.setEnabled(true); // Enable the text field
			playerNameComboBox.setEnabled(false); // Disable the combo box
		});
		returningPlayerButton.addActionListener(e -> {
			cardLayout.show(cardPanel, "ReturningPlayer");
			playerNameComboBox.setEnabled(true); // Enable the combo box
			newPlayerNameField.setEnabled(false); // Disable the text field
		});

		// Initially, neither input is enabled until a radio button is selected
		newPlayerNameField.setEnabled(false);
		playerNameComboBox.setEnabled(false);

		// Initially, neither input is shown until a radio button is selected
		cardLayout.show(cardPanel, "");

		pack();
		setLocationRelativeTo(parent);
	} // LoginDialog(JFrame, Scoreboard)

	private void refreshPlayerNames() {
		playerNameComboBox.removeAllItems();
		for (int i = 0; i < scoreboard.getNumberOfPlayers(); i++) {
			playerNameComboBox.addItem(scoreboard.getPlayer(i).getName());
		}
	}

	private void onOk() {
		if (newPlayerNameField.isVisible()) {
			selectedPlayerName = newPlayerNameField.getText();
			if (selectedPlayerName == null || selectedPlayerName.trim().isEmpty()) {
				JOptionPane.showMessageDialog(this, "Please enter a valid name.");
				return;
			}
			// Add the new player to the scoreboard if they don't already exist
			if (!scoreboard.hasPlayer(selectedPlayerName)) {
				scoreboard.addPlayer(new Player(selectedPlayerName));
				refreshPlayerNames(); // Refresh the player names after adding a new player
			}
			if (newPlayerNameField.isVisible()) {
				
			} else if (playerNameComboBox.isVisible()) {
				
			} else {
				JOptionPane.showMessageDialog(this, "Please select New Player or Returning Player.");
				return;
			}
		} else {
			if (playerNameComboBox.getItemCount() == 0) {
				JOptionPane.showMessageDialog(this,
						"No player has been saved yet. Pick the new player option to start a new game!");
				return;
			}
			selectedPlayerName = (String) playerNameComboBox.getSelectedItem();
		}

		dispose();
	}

	public String getSelectedPlayerName() {
		return selectedPlayerName;
	}
}
