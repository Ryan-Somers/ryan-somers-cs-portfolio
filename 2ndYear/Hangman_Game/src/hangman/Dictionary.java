package hangman;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.Random;
import hangman.linked_data_structures.SinglyLinkedList;

public class Dictionary {
	private SinglyLinkedList<String> words = new SinglyLinkedList<>();
	private static final int MIN_LENGTH = 3;
	private static final int MAX_LENGTH = 15;
	private static final int MIN_GUESSABLE_CHARACTERS = 2;

	public Dictionary() {
		String filename = "wordslist.txt";
		try (BufferedReader br = new BufferedReader(new FileReader(filename))) {
			String line;
			while ((line = br.readLine()) != null) {
				if (isValidWord(line)) {
					getWords().add(line.trim());
				}
			}
		} catch (IOException e) {

		}

		if (getWords().getLength() == 0) {

			throw new IllegalStateException("No valid words found in the dictionary.");

		}
	} // Dictionary()

	public boolean isValidWord(String word) {
		if (word == null || word.length() < getMinLength() || word.length() > MAX_LENGTH) {
			return false;
		}

		int guessableCharacters = 0;
		for (char c : word.toCharArray()) {
			if (Character.isLetter(c)) {
				guessableCharacters++;
			}
		} // for

		return guessableCharacters >= MIN_GUESSABLE_CHARACTERS;
	} // isValidWord(String)

	public String getRandomWord() {
		if (getWords().getLength() == 0) {
			throw new IllegalStateException("No words left in the dictionary.");
		}

		int index = new Random().nextInt(getWords().getLength());
		String selectedWord = getWords().getElementAt(index);

		// Remove the selected word from the list
		removeWord(selectedWord);

		return selectedWord;
	} // getRandomWord()

	public void removeWord(String word) {
		for (int i = 0; i < getWords().getLength(); i++) {
			if (getWords().getElementAt(i).equals(word)) {
				getWords().remove(i);
				break;
			}
		}
	} // removeWord(String)

	public static int getMinLength() {
		return MIN_LENGTH;
	}

	public SinglyLinkedList<String> getWords() {
		return words;
	}

	public void setWords(SinglyLinkedList<String> words) {
		this.words = words;
	}

	public static int getMaxLength() {
		return MAX_LENGTH;
	}
}
