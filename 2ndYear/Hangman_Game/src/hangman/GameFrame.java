package hangman;

import java.awt.*;
import java.awt.event.*;
import java.awt.image.BufferedImage;
import java.io.FileNotFoundException;
import java.util.ArrayList;

import javax.swing.*;
import javax.swing.border.Border;
import javax.swing.border.EmptyBorder;
import javax.swing.border.LineBorder;
import javax.swing.plaf.basic.BasicButtonUI;

import hangman.Game.GuessResult;

public class GameFrame extends JFrame {

	private static final long serialVersionUID = 1L;
	private static final int IMAGE_WIDTH = 500;
	private static final int IMAGE_HEIGHT = 400;
	private ArrayList<JButton> letterButtons = new ArrayList<>();
	private JButton hintButton;
	private JButton playAgainButton;
	private JPanel contentPane;
	private JPanel imagePanel = new JPanel();
	private JPanel topPanel;
	private JPanel wordPanel;
	private JPanel centerWordPanel;
	private String currentPlayerName;
	private JLabel hangmanLabel;
	private JLabel welcomeLabel;
	private JLabel wordLabel;
	private JLabel wrongGuessesLabel;
	private JLabel messageLabel;
	private JDialog gameOverDialog;
	private Game game;
	private Scoreboard scoreboard;
	private Dictionary dictionary;
	private int currentImageIndex = 0;

	private ImageIcon[] hangmanImages = {
			new ImageIcon(new ImageIcon("./hangmanStart.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)),
			new ImageIcon(new ImageIcon("./Frame1.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)),
			new ImageIcon(new ImageIcon("./Frame2.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)),
			new ImageIcon(new ImageIcon("./Frame3.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)),
			new ImageIcon(new ImageIcon("./Frame4.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)),
			new ImageIcon(new ImageIcon("./Frame5.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)),
			new ImageIcon(new ImageIcon("./FrameGone.png").getImage().getScaledInstance(IMAGE_WIDTH, IMAGE_HEIGHT,
					Image.SCALE_DEFAULT)), };

	public static void main(String[] args) {
		EventQueue.invokeLater(() -> {
			try {

				GameFrame frame = new GameFrame();
				frame.setResizable(false);
				frame.setVisible(true);
			} catch (Exception e) {
				e.printStackTrace();
			}
		});
	} // main(String[])

	public GameFrame() {
		setBackground(Color.DARK_GRAY);
		scoreboard = GameStateManager.loadState();

		LoginDialog loginDialog = new LoginDialog(this, scoreboard);
		loginDialog.setVisible(true);
		String playerName = loginDialog.getSelectedPlayerName();
		if (playerName == null) {
			System.exit(0);
		}

		currentPlayerName = playerName;

		try {
			this.dictionary = new Dictionary();
		} catch (Exception e) {
			JOptionPane.showMessageDialog(this, "Error initializing dictionary.", "Error", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}

		Player currentPlayer = scoreboard.getPlayerByName(currentPlayerName);
		if (currentPlayer != null) {
			game = currentPlayer.getGameState();
		}

		if (game == null) {
			try {
				this.dictionary = new Dictionary();
				String randomWord = dictionary.getRandomWord();
				game = new Game(randomWord);
				currentImageIndex = 0;
			} catch (IllegalStateException e) {
				JOptionPane.showMessageDialog(this, e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
				System.exit(1);
			}
		} else {
			currentImageIndex = game.getCurrentImageIndex();

		}

		// Initialize UI components
		initializeMenuBar();
		initializeContentPane();
		// Now check the hint state
		if (game.isHintUsed()) {
			hintButton.setEnabled(false);
		}
		// Update the welcome message
		welcomeLabel.setText("WELCOME TO HANGMAN " + currentPlayerName.toUpperCase() + "!");

		// Update the UI to reflect the loaded game state or the new game state
		updateLetterButtonsState();
		updateWrongGuessesLabel();

		// Set up the frame
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 900, 650);
		setLocationRelativeTo(null);
		imagePanel.setPreferredSize(new Dimension(IMAGE_WIDTH, IMAGE_HEIGHT));

		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent e) {
				GameStateManager.saveState(game, scoreboard, currentPlayerName, currentImageIndex);
			} // windowClosing(WindowEvent)
		}); // addWindowlistener()

	} // GameFrame()

	private void initializeMenuBar() {
		JMenuBar menuBar = new JMenuBar();
		setJMenuBar(menuBar);

		JMenu gameMenu = new JMenu("Game");
		menuBar.add(gameMenu);
		JMenu helpMenu = new JMenu("Help");
		menuBar.add(helpMenu);

		JMenuItem rulesMenuItem = new JMenuItem("Rules");
		helpMenu.add(rulesMenuItem);
		rulesMenuItem.addActionListener(e -> showRules());

		JMenuItem showScoreboard_MenuItem = new JMenuItem("Show Scoreboard");
		gameMenu.add(showScoreboard_MenuItem);
		showScoreboard_MenuItem.addActionListener(e -> {
			ScoreboardDialog scoreboardDialog = new ScoreboardDialog(this, scoreboard);
			scoreboardDialog.setVisible(true);
		}); // showScoreboard addActionListener

		JMenuItem startNewGame_MenuItem = new JMenuItem("Start a New Game");
		gameMenu.add(startNewGame_MenuItem);
		startNewGame_MenuItem.addActionListener(e -> {
			restartGame();
		}); // startNewGame addActionListener

		JMenuItem quitMenuItem = new JMenuItem("Quit");
		gameMenu.add(quitMenuItem);
		quitMenuItem.addActionListener(e -> {
			GameStateManager.saveState(game, scoreboard, currentPlayerName, currentImageIndex);
			System.exit(-1);
		}); // quitMenu addActionListener

	} // initializeMenuBar()

	private void initializeContentPane() {
		contentPane = new JPanel();
		contentPane.setForeground(Color.DARK_GRAY);
		contentPane.setBackground(Color.DARK_GRAY);

		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		contentPane.setLayout(new BorderLayout(0, 0));
		setContentPane(contentPane);

		hintButton = new JButton("Hint");
		hintButton.setFont(new Font("Krungthep", Font.PLAIN, 13));
		hintButton.addActionListener(e -> {
			char hintLetter = game.getHint();
			game.guessLetter(hintLetter);
			updateDisplay();
			hintButton.setEnabled(false); // Disable the hint button after it's used

			// Disable the button for the hint letter
			letterButtons.get(hintLetter - 'a').setEnabled(false);
		});

		topPanel = new JPanel(new BorderLayout());
		topPanel.setBackground(Color.DARK_GRAY);
		topPanel.add(hintButton, BorderLayout.EAST);
		contentPane.add(topPanel, BorderLayout.NORTH);

		welcomeLabel = new JLabel("WELCOME TO HANGMAN " + currentPlayerName.toUpperCase() + "!");

		welcomeLabel.setForeground(Color.WHITE);
		welcomeLabel.setBackground(Color.DARK_GRAY);
		welcomeLabel.setFont(new Font("Krungthep", Font.BOLD, 16));
		welcomeLabel.setHorizontalAlignment(SwingConstants.CENTER);
		topPanel.add(welcomeLabel, BorderLayout.CENTER);

		// Letter Panel
		JPanel letterPanel = new JPanel();
		letterPanel.setBackground(Color.DARK_GRAY);
		letterPanel.setLayout(new GridLayout(2, 13));

		for (char c = 'A'; c <= 'Z'; c++) {
			JButton button = new JButton(String.valueOf(c));
			button.setFont(new Font("Arial", Font.BOLD, 16));
			button.setFocusPainted(false);

			// Set the button background to black
			button.setBackground(Color.BLACK);

			// Set the button text color to white
			button.setForeground(Color.WHITE);

			button.setOpaque(false);

			button.setPreferredSize(new Dimension(40, 40));

			// Custom paint to fill the button with its background color and make it rounded
			button.setUI(new BasicButtonUI() {
				@Override
				public void paint(Graphics g, JComponent c) {
					if (button.isContentAreaFilled()) {
						Graphics2D g2 = (Graphics2D) g.create();
						g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
						g2.setColor(button.getBackground());
						g2.fillRoundRect(0, 0, c.getWidth(), c.getHeight(), 20, 20);
						g2.dispose();
					}
					super.paint(g, c);
				}
			});

			// Set a custom rounded border
			button.setBorder(new LineBorder(Color.DARK_GRAY) {
				@Override
				public void paintBorder(Component c, Graphics g, int x, int y, int width, int height) {
					Graphics2D g2 = (Graphics2D) g.create();
					g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
					g2.setColor(getLineColor());
					g2.drawRoundRect(x, y, width - 1, height - 1, 20, 20);
					g2.dispose();
				}
			});

			button.addActionListener(e -> {
				char guessedLetter = button.getText().charAt(0);
				handleGuess(guessedLetter, button);
			});
			letterButtons.add(button);
			letterPanel.add(button);

		} // for()

		contentPane.add(letterPanel, BorderLayout.SOUTH);

		// Create a new JPanel with BorderLayout
		imagePanel = new JPanel(new BorderLayout());
		imagePanel.setForeground(Color.DARK_GRAY);

		// Image Panel
		hangmanLabel = new JLabel();

		hangmanLabel.setIcon(hangmanImages[currentImageIndex]);
		Border thickBlackBorder = BorderFactory.createLineBorder(Color.DARK_GRAY, 5); // 5 pixels thick
		hangmanLabel.setBorder(thickBlackBorder);
		imagePanel.add(hangmanLabel, BorderLayout.CENTER);

		// Add the imagePanel to your frame or another container
		contentPane.add(imagePanel, BorderLayout.WEST);

		// WORD PANEL
		wordPanel = new JPanel();
		wordPanel.setBackground(Color.DARK_GRAY);
		wordPanel.setForeground(Color.DARK_GRAY);
		wordPanel.setBorder(new EmptyBorder(10, 50, 10, 50)); // top, left, bottom, right
		wordLabel = new JLabel();
		wordLabel.setVerticalAlignment(SwingConstants.TOP);
		wordLabel.setForeground(Color.WHITE);
		wordLabel.setHorizontalAlignment(SwingConstants.CENTER);
		wordLabel.setBackground(Color.WHITE);
		updateDisplay();
		wordPanel.setLayout(new GridLayout(0, 1, 0, 0));

		JLabel lblNewLabel_1 = new JLabel("YOUR WORD:");
		lblNewLabel_1.setVerticalAlignment(SwingConstants.BOTTOM);
		lblNewLabel_1.setHorizontalAlignment(SwingConstants.CENTER);
		lblNewLabel_1.setForeground(Color.WHITE);
		lblNewLabel_1.setFont(new Font("Krungthep", Font.PLAIN, 13));
		wordPanel.add(lblNewLabel_1);
		wordLabel.setFont(new Font("Arial", Font.BOLD, 24));
		wordPanel.add(wordLabel);
		imagePanel.setBackground(Color.decode("#f3f3f3"));

		// Create a new JPanel to center the wordPanel vertically
		centerWordPanel = new JPanel();
		centerWordPanel.setLayout(new BoxLayout(centerWordPanel, BoxLayout.Y_AXIS));
		centerWordPanel.setBackground(Color.DARK_GRAY);

		centerWordPanel.add(Box.createVerticalGlue());
		centerWordPanel.add(wordPanel);
		centerWordPanel.add(Box.createVerticalGlue());

		// Create a new JPanel to center the centerWordPanel horizontally
		JPanel eastPanel = new JPanel(new GridBagLayout());
		eastPanel.setBackground(Color.DARK_GRAY);

		GridBagConstraints gbc = new GridBagConstraints();
		gbc.weightx = 1.0;
		gbc.weighty = 1.0;
		gbc.fill = GridBagConstraints.BOTH;

		eastPanel.add(centerWordPanel, gbc);

		// Add the eastPanel to the CENTER of the contentPane
		contentPane.add(eastPanel, BorderLayout.CENTER);

		imagePanel.setBackground(Color.decode("#f3f3f3"));

		wrongGuessesLabel = new JLabel();
		wrongGuessesLabel.setForeground(Color.WHITE);
		wrongGuessesLabel.setFont(new Font("Krungthep", Font.PLAIN, 16));
		topPanel.add(wrongGuessesLabel, BorderLayout.WEST);

	} // initializeContentPane()

	private void handleGuess(char guessedLetter, JButton button) {
		button.setEnabled(false); // Disable the button after it's been clicked

		GuessResult result = game.handleGuess(guessedLetter);
		updateDisplay();

		if (game.isOnlyOneLetterLeft()) {
			hintButton.setEnabled(false);
		}

		switch (result) {
		case LOST:
			updateHangmanImage();
			handleGameOver(false);
			break;
		case WON:
			handleGameOver(true);
			break;
		case INCORRECT_GUESS:
			updateHangmanImage();
			updateWrongGuessesLabel(); // Only update the label for incorrect guesses
			break;
		default:
			break;
		}

		Player currentPlayer = scoreboard.getPlayerByName(currentPlayerName);
		currentPlayer.setGameState(game); // Update the player's game state
	} // handleGuess(char, JButton)

	private void handleGameOver(boolean isWin) {

		String message;
		if (isWin) {
			message = "Congratulations, you guessed the word!";
		} else {
			String wordString = "";
			for (int i = 0; i < game.getWordToGuess().getLength(); i++) {
				wordString += game.getWordToGuess().getElementAt(i);
			}
			message = "Sorry, you lost! The word was: " + wordString;

		}

		// Create a custom JDialog
		gameOverDialog = new JDialog(this, "Game Over", true);
		gameOverDialog.getContentPane().setLayout(new BorderLayout());
		gameOverDialog.setSize(300, 150);
		gameOverDialog.setLocationRelativeTo(this);

		// Add message
		messageLabel = new JLabel(message, SwingConstants.CENTER);
		gameOverDialog.getContentPane().add(messageLabel, BorderLayout.CENTER);

		// Add "Play Again" button
		playAgainButton = new JButton("Play Again");
		playAgainButton.addActionListener(e -> {
			currentImageIndex = -1;
			gameOverDialog.dispose();
			restartGame();

		}); // playAgainButton addActionListener
		gameOverDialog.getContentPane().add(playAgainButton, BorderLayout.SOUTH);

		// Show the dialog
		gameOverDialog.setVisible(true);

		scoreboard.gamePlayed(currentPlayerName, isWin);

	} // HandleGameOver(boolean)

	private void showRules() {
		String rulesText = "Here are the rules for Hangman:\n"
				+ "1. The computer selects a word at random from the dictionary.\n"
				+ "2. The player tries to guess the word letter by letter.\n"
				+ "3. For each incorrect guess, a part of the hangman is drawn.\n"
				+ "4. The player loses if the entire hangman is drawn.\n"
				+ "5. The player wins if they guess the word before the hangman is completed.\n\n"
				+ "* Please note that the hint button can only be used once and it can't assist you for the last letter! *";

		JTextArea textArea = new JTextArea(rulesText);
		textArea.setEditable(false);
		textArea.setWrapStyleWord(true);
		textArea.setLineWrap(true);
		textArea.setCaretPosition(0);
		textArea.setColumns(30);
		textArea.setRows(10);
		JScrollPane scrollPane = new JScrollPane(textArea);

		JOptionPane.showMessageDialog(this, scrollPane, "Rules", JOptionPane.INFORMATION_MESSAGE);
	} // showRules()

	private void updateDisplay() {
		String currentDisplay = game.getCurrentDisplay();
		wordLabel.setText(currentDisplay);
		
	} // updateDisplay()

	private void updateHangmanImage() {
		if (currentImageIndex < hangmanImages.length - 1) {
			currentImageIndex++; // Increment the index
			hangmanLabel.setIcon(hangmanImages[currentImageIndex]);
		} else {
			currentImageIndex = hangmanImages.length - 1; // Ensure it doesn't exceed the array length
		}

	} // updateHangmanImage()

	private void restartGame() {
		wrongGuessesLabel.setText("");
		currentImageIndex = 0; // Start with the first image
		hangmanLabel.setIcon(hangmanImages[currentImageIndex]);

		// Remove the word that was just played
		String wordPlayed = game.getCurrentWord();
		dictionary.removeWord(wordPlayed);

		// Reset the game state
		enableAllLetterButtons();

		// Reinitialize the game
		try {
			String randomWord = dictionary.getRandomWord();
			game = new Game(randomWord);
		} catch (IllegalStateException e) {
			JOptionPane.showMessageDialog(this, e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
			System.exit(1);
		}

		enableAllLetterButtons();
		hintButton.setEnabled(true);
		updateDisplay();

		// Directly set the hangmanLabel to the first image
		hangmanLabel.setIcon(hangmanImages[0]);
	} // restartGame()

	private void enableAllLetterButtons() {
		for (JButton button : letterButtons) {
			button.setEnabled(true);
		} // for()
	} // enableAllLetterButtons

	private void updateLetterButtonsState() {
		for (JButton button : letterButtons) {
			char letter = button.getText().charAt(0);
			if (game.isLetterAlreadyGuessed(letter)) {
				button.setEnabled(false);
			}
		} // for()
	} // updateLetterButtonsState()

	private void updateWrongGuessesLabel() {
		String wrongGuesses = game.getWrongGuesses();
		wrongGuessesLabel.setText("Wrong guesses: " + wrongGuesses);
	} // updateWrongGuessesLabel()

}
