package sudokugame;

import java.awt.Color;
import java.awt.EventQueue;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.swing.BorderFactory;
import javax.swing.JFileChooser;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JTextField;
import javax.swing.SwingConstants;

/**
 * Title:       SudokuFrame
 * Description: This is the GUI for the SudokuGame
 *              Have fun! 
 * 4 14, 2023  
 */
public class SudokuFrame extends JFrame implements ActionListener {

	private static final int GRID_SIZE = 9;
	private static final int PANEL_SIZE = 3;
	SudokuGame game1 = new SudokuGame();
	private JPanel contentPane;
	private JMenuBar menuBar = new JMenuBar();
	private JMenu fileMenu = new JMenu("File");
	private JMenuItem exitMenuItem = new JMenuItem("Exit");
	private JMenu openGame = new JMenu("Game");
	private JMenuItem openFile = new JMenuItem("Open File");
	private JMenu helpMenu = new JMenu("Help");
	private JMenuItem aboutMenuItem = new JMenuItem("About");
	private JMenuItem saveMenuItem = new JMenuItem("Save Game");
	private JMenuItem undoMenuItem = new JMenuItem("Undo Move");
	private JMenuItem rulesMenuItem = new JMenuItem("Rules");
	private boolean isFileSelected;
	private JTextField[][] fields;
	private String fileName = "";
	private Font font = new Font("Arial", Font.BOLD, 20);
	// used ArrayList to do the undo function
	private List<Map<JTextField, String>> moveHistory = new ArrayList<>();
	private boolean undoPerformed = false;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					SudokuFrame frame = new SudokuFrame();
					frame.setVisible(true);

				}
				catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	} // main(String)

	/**
	 * Create the frame.
	 */
	public SudokuFrame() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 470, 470);
		contentPane = new JPanel();
		contentPane.setBorder(BorderFactory.createLineBorder(Color.BLACK, 5));
		setLocationRelativeTo(null); // center frame on screen

		setContentPane(contentPane);
		setTitle("Sudoku Game");
		setJMenuBar(menuBar);
		menuBar.add(fileMenu);
		fileMenu.add(openFile);
		openFile.addActionListener(this);
		fileMenu.add(exitMenuItem);
		exitMenuItem.addActionListener(this);
		menuBar.add(openGame);
		openGame.add(saveMenuItem);
		openGame.add(undoMenuItem);
		undoMenuItem.addActionListener(this);
		saveMenuItem.addActionListener(this);
		menuBar.add(helpMenu);
		helpMenu.add(aboutMenuItem);
		helpMenu.add(rulesMenuItem);
		aboutMenuItem.addActionListener(this);
		rulesMenuItem.addActionListener(this);
		initialize();
	} // SudokuFrame()

	public void saveFile(String filename) {
		try {
			BufferedWriter writer = new BufferedWriter(new FileWriter(fileName));
			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					JTextField field = fields[row][col];
					if (!field.isEnabled()) { // save only disabled fields
						String value = field.getText();
						if (!value.isEmpty()) {
							int num = Integer.parseInt(value);
							writer.write(Integer.toString(num));
						}
						else {
							writer.write("*");
						}
					}
					else {
						writer.write("*"); // cell is enabled, so write * to the file
					}
					if (col < 8) {
						writer.write("~");
					}
				}
				writer.newLine();
			}
			writer.close();
			JOptionPane.showMessageDialog(this, "File was saved successfully",
					"Successful Save", JOptionPane.INFORMATION_MESSAGE);
		}
		catch (IOException e) {
			JOptionPane.showMessageDialog(this, "File was not saved successfully",
					"Save Not Successful", JOptionPane.ERROR_MESSAGE);

		}
	} // saveFile(String)

	private JPanel createPanel() {
		JPanel panel = new JPanel();
		panel.setLayout(new GridLayout(PANEL_SIZE, PANEL_SIZE));
		panel.setBackground(Color.GRAY);
		panel.setBorder(BorderFactory.createLineBorder(Color.BLACK));
		return panel;
	} // createPanel()

	public void initialize() {

		// Create the grid of text fields
		fields = new JTextField[GRID_SIZE][GRID_SIZE];
		getContentPane().setLayout(new GridLayout(PANEL_SIZE, PANEL_SIZE));

		for (int panelRow = 0; panelRow < PANEL_SIZE; panelRow++) {
			for (int panelCol = 0; panelCol < PANEL_SIZE; panelCol++) {
				JPanel panel = createPanel();
				getContentPane().add(panel);
				for (int row = panelRow * PANEL_SIZE; row < (panelRow + 1)
						* PANEL_SIZE; row++) {
					for (int col = panelCol * PANEL_SIZE; col < (panelCol + 1)
							* PANEL_SIZE; col++) {
						JTextField field = new JTextField();
						field.setFont(font);
						field.setHorizontalAlignment(JTextField.CENTER);
						fields[row][col] = field;
						panel.add(field);
					}
				}
			}
		}
	} // initialize()

	public void loadFile(String filename)
			throws IOException, FileNotFoundException {
		BufferedReader br = new BufferedReader(new FileReader(filename));
		String line;
		int row = 0;
		while ((line = br.readLine()) != null && row < 9) {
			String[] values = line.split("~");
			for (int col = 0; col < 9 && col < values.length; col++) {
				String value = values[col];
				if (!value.equals("*") && !value.equals("")) {
					int num = 0;
					try {
						num = Integer.parseInt(value);
					}

					catch (NumberFormatException e) {
						JOptionPane.showMessageDialog(this,
								"String found. Please take String away and try again!",
								"String Found!", JOptionPane.ERROR_MESSAGE);

					}
					fields[row][col].setText(value);
					game1.getBoard()[row][col] = num;
					fields[row][col].setEnabled(false);
				}
			}
			row++;
		}
		br.close();
	} // loadFile(String)

	private void selectFileName() throws IOException, FileNotFoundException {
		File sudokuFile;
		JFileChooser file = new JFileChooser(".//");
		FileNameExtensionFilter filter = new FileNameExtensionFilter(
				"Text File (.txt)", "txt");
		file.setAcceptAllFileFilterUsed(false);
		file.setFileFilter(filter);
		int i = file.showOpenDialog(this);
		if (i == JFileChooser.APPROVE_OPTION) {
			sudokuFile = file.getSelectedFile();
			fileName = sudokuFile.getName();
		}
		try {
			loadFile(fileName);
			JOptionPane.showMessageDialog(this, "Have a great game!",
					"Successful Open", JOptionPane.INFORMATION_MESSAGE);
		}
		catch (FileNotFoundException e) {
			JOptionPane.showMessageDialog(this,
					"Oops! The file was not opened properly.", "File Not Opened",
					JOptionPane.ERROR_MESSAGE);
		}

	} // selectFileName()

	private void inputEntered(int row, int col) {
		undoPerformed = false;
		JTextField field = fields[row][col];
		String previousValue = field.getText();

		try {
			int input = Integer.parseInt(fields[row][col].getText());

			if (input < 1 || input > 9) {
				throw new NumberFormatException();
			}
			if (game1.isValid(game1.getBoard(), input, col, row)) {
				JOptionPane.showMessageDialog(this, "Value in col/row/box.",
						"Value in col/row/box", JOptionPane.ERROR_MESSAGE);
				field.setEnabled(true);
			}
			else {
				// Store the previous value of the field
				Map<JTextField, String> previousValues = new HashMap<>();
				previousValues.put(field, previousValue);
				moveHistory.add(previousValues);

				// Update the value of the field
				field.setText(Integer.toString(input));
				field.setEnabled(false);

			}

		}
		catch (NumberFormatException e) {
			JOptionPane.showMessageDialog(this,
					"Cannot enter a letter, only 1-9 integers.", "Only enter integers.",
					JOptionPane.ERROR_MESSAGE);
			field.setEnabled(true);
		}
		if (game1.isFrameComplete(fields)) {
			JOptionPane.showMessageDialog(this,
					"Wow! I am impressed. You are a sudoku wizard yourself!",
					"Congratulations", JOptionPane.INFORMATION_MESSAGE);
		}
	} // inputEntered(int, int)

	private void undo() {
		if (moveHistory.isEmpty()) {
			JOptionPane.showMessageDialog(this, "Nothing to undo.", "Undo Error",
					JOptionPane.ERROR_MESSAGE);
			return;
		}
		if (undoPerformed) {
			JOptionPane.showMessageDialog(this, "You can only undo once per move.",
					"Undo Error", JOptionPane.ERROR_MESSAGE);
		}
		else {
			// Get the most recent move's previous values map
			Map<JTextField, String> previousValues = moveHistory
					.remove(moveHistory.size() - 1);
			for (JTextField field : previousValues.keySet()) {
				String previousValue = previousValues.get(field);
				field.setText(previousValue);
				field.setEnabled(true);
			}
			undoPerformed = true;
		}
	} // undo()

	@Override
	public void actionPerformed(ActionEvent e) {
		if (e.getSource() == exitMenuItem) {
			JOptionPane.showMessageDialog(this,
					"Closing Game Down. Have a great day!", "Game Closing",
					JOptionPane.INFORMATION_MESSAGE);
			System.exit(0);
		} // exitMenu
		else
			if (e.getSource() == openFile) {
				if (!isFileSelected) {
					try {
						selectFileName();
					}
					catch (IOException e1) {

					}
				}

				SudokuFrame frame = new SudokuFrame();
				frame.setVisible(true);
				frame.setDefaultCloseOperation(DISPOSE_ON_CLOSE);

				// actionListener for textfields
				for (int row = 0; row < 9; row++) {
					for (int col = 0; col < 9; col++) {
						final int currentRow = row;
						final int currentCol = col;
						fields[row][col].addActionListener(new ActionListener() {
							@Override
							public void actionPerformed(ActionEvent e) {
								inputEntered(currentRow, currentCol);
							}
						});
					}
				}

			} // openFile
			else
				if (e.getSource() == saveMenuItem) {
					saveFile(fileName);
				} // save
				else
					if (e.getSource() == aboutMenuItem) {
						JOptionPane.showMessageDialog(this, new SudokuFrame_About(),
								"About", JOptionPane.PLAIN_MESSAGE);
					} // aboutMenu
					else
						if (e.getSource() == rulesMenuItem) {
							JOptionPane.showMessageDialog(this, new SudokuFrame_HelpFrame(),
									"Rules", JOptionPane.PLAIN_MESSAGE);
						} // rulesMenu
						else
							if (e.getSource() == undoMenuItem) {
								undo();
							} // undoMenu
	} // actionPerformed(ActionEvent)

}
