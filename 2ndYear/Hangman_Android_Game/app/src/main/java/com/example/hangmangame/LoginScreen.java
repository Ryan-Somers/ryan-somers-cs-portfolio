package com.example.hangmangame;

import android.content.Intent;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.util.ArrayList;
import java.util.List;

import HangmanLogic.GameStateManager;
import HangmanLogic.Player;
import HangmanLogic.Scoreboard;

public class LoginScreen extends AppCompatActivity {
    private Spinner spinnerExistingPlayers;
    private EditText editTextNewPlayer;
    private Button playGameButton;
    private Button playNewPlayerButton;
    private Button playReturningPlayerButton;
    private Scoreboard scoreboard;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login_screen2);

        initializeUI();
    }

    private void initializeUI() {
        // Load or create a new scoreboard
        scoreboard = GameStateManager.loadState(this);
        if (scoreboard == null) {
            scoreboard = new Scoreboard();
        }

        spinnerExistingPlayers = findViewById(R.id.spinner1);
        editTextNewPlayer = findViewById(R.id.editTextText3);
        playNewPlayerButton = findViewById(R.id.playNewPlayerButton);
        playReturningPlayerButton = findViewById(R.id.playReturningPlayerButton);
        playGameButton = findViewById(R.id.playGameBtn);



        // Set initial states of UI components
        playGameButton.setEnabled(false);
        editTextNewPlayer.setEnabled(false);
        spinnerExistingPlayers.setEnabled(false);
        playNewPlayerButton.setEnabled(true);
        playReturningPlayerButton.setEnabled(true);

        setupButtonListeners();
        updateSpinner();
    }

    private void setupButtonListeners() {
        playNewPlayerButton.setOnClickListener(view -> {
            togglePlayerSelection(true); // Enable New Player mode
        });

        playReturningPlayerButton.setOnClickListener(view -> {
            togglePlayerSelection(false); // Enable Returning Player mode
        });

        playGameButton.setOnClickListener(view -> handlePlayGame());
    }

    private void togglePlayerSelection(boolean isNewPlayer) {
        if (isNewPlayer) {
            // New Player mode
            playGameButton.setEnabled(true);
            editTextNewPlayer.setEnabled(true);
            spinnerExistingPlayers.setEnabled(false);
            playReturningPlayerButton.setEnabled(true);
            playNewPlayerButton.setEnabled(false);
        } else {
            // Returning Player mode
            playGameButton.setEnabled(true);
            editTextNewPlayer.setEnabled(false);
            spinnerExistingPlayers.setEnabled(true);
            playNewPlayerButton.setEnabled(true);
            playReturningPlayerButton.setEnabled(false);
        }
    }

    private void handlePlayGame() {
        if (isNewPlayer()) {
            String playerName = editTextNewPlayer.getText().toString().trim();
            if (playerName.isEmpty()) {
                Toast.makeText(this, "Please enter a name", Toast.LENGTH_SHORT).show();
                return;
            }

            if (!scoreboard.hasPlayer(playerName)) {
                scoreboard.addPlayer(new Player(playerName));
                GameStateManager.saveScoreboard(this, scoreboard);
            }

            startGame(playerName);
        } else {
            if (spinnerExistingPlayers.getSelectedItem() == null) {
                Toast.makeText(this, "No returning players available", Toast.LENGTH_SHORT).show();
                return;
            }

            String playerName = spinnerExistingPlayers.getSelectedItem().toString();
            startGame(playerName);
        }
    }





    private boolean isNewPlayer() {
        return editTextNewPlayer.isEnabled();
    }

    private void startGame(String playerName) {
        Intent intent = new Intent(this, GameScreen.class);
        intent.putExtra("playerName", playerName);
        startActivity(intent);
    }

    private void updateSpinner() {
        List<String> playerNames = new ArrayList<>();
        for (int i = 0; i < scoreboard.getNumberOfPlayers(); i++) {
            playerNames.add(scoreboard.getPlayer(i).getName());
        }
        ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, playerNames);
        spinnerExistingPlayers.setAdapter(adapter);

        // Disable spinner if there are no players
        spinnerExistingPlayers.setEnabled(!playerNames.isEmpty());
    }

    @Override
    protected void onResume() {
        super.onResume();
        initializeUI(); // Reset UI elements every time the screen is displayed
    }
}
