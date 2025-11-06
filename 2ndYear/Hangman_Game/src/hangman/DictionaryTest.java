package hangman;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class DictionaryTest {

	private Dictionary dictionary;

	@BeforeEach
	void setUp() {
		dictionary = new Dictionary();
	}

	@Test
	void testDictionaryLoadsWords() {
		assertTrue(dictionary.getWords().getLength() > 0, "Dictionary should load words from the file");
	}

	@Test
	void testGetRandomWordReturnsValidWord() {
		String word = dictionary.getRandomWord();
		assertNotNull(word, "Returned word should not be null");
		assertTrue(word.length() >= Dictionary.getMinLength() && word.length() <= Dictionary.getMaxLength(),
				"Returned word should be of valid length");
	}

	@Test
	void testRemoveWord() {
		String word = dictionary.getRandomWord();
		dictionary.removeWord(word);
		assertFalse(isWordInDictionary(word), "Word should be removed from the dictionary");
	}

	@Test
	void testGetRandomWordDoesNotReturnSameWordTwice() {
		String word1 = dictionary.getRandomWord();
		String word2 = dictionary.getRandomWord();
		assertNotEquals(word1, word2, "getRandomWord should not return the same word twice");
	}

	@Test
	void testIsValidWordFiltersOutInvalidWords() {
		assertFalse(dictionary.isValidWord("a"), "Word is too short");
		assertFalse(dictionary.isValidWord("thisisaveryverylongword"), "Word is too long");
		assertFalse(dictionary.isValidWord("a1"), "Word has too few guessable characters");
	}

	private boolean isWordInDictionary(String word) {
		for (int i = 0; i < dictionary.getWords().getLength(); i++) {
			if (dictionary.getWords().getElementAt(i).equals(word)) {
				return true;
			}
		}
		return false;
	}
}
