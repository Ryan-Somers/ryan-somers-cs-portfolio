package com.example.hangmangame;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupMenu;
import android.widget.ScrollView;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import HangmanLogic.GameStateManager;
import HangmanLogic.Player;
import HangmanLogic.Scoreboard;

public class ScoreboardScreen extends AppCompatActivity {


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_scoreboard_screen);

        // Layout to hold everything
        LinearLayout mainLayout = new LinearLayout(this);
        mainLayout.setOrientation(LinearLayout.VERTICAL);
        mainLayout.setLayoutParams(new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT));
        mainLayout.setGravity(Gravity.CENTER_HORIZONTAL);
        mainLayout.setPadding(16, 16, 16, 16);

        // Scoreboard Title
        TextView title = new TextView(this);
        title.setText("SCOREBOARD");
        title.setTextColor(Color.BLACK);
        title.setTextSize(24);
        title.setGravity(Gravity.CENTER);
        mainLayout.addView(title);

        ScrollView scrollView = new ScrollView(this);
        TableLayout table = new TableLayout(this);
        table.setStretchAllColumns(true);
        table.setShrinkAllColumns(true);

        // Header Row
        TableRow headerRow = new TableRow(this);
        String[] headers = {"Player Name", "Games Played", "Games Won"};
        for (String header : headers) {
            TextView tv = new TextView(this);
            tv.setText(header);
            tv.setTextColor(Color.BLACK);
            tv.setTextSize(18);
            tv.setPadding(8, 8, 8, 8);
            tv.setGravity(Gravity.CENTER);
            headerRow.addView(tv);
        }
        table.addView(headerRow);

        List<Player> players = getPlayers();
        for (Player player : players) {
            TableRow row = new TableRow(this);
            row.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.MATCH_PARENT, TableRow.LayoutParams.WRAP_CONTENT));
            row.setPadding(8, 8, 8, 8);
            row.setBackgroundColor(Color.parseColor("#CCDDFF"));

            TextView name = new TextView(this);
            name.setText(player.getName());
            name.setTextSize(18);
            name.setTextColor(Color.BLACK);
            name.setGravity(Gravity.CENTER);
            name.setPadding(8, 8, 8, 8);

            TextView gamesPlayed = new TextView(this);
            gamesPlayed.setText(String.valueOf(player.getNumberGamesPlayed()));
            gamesPlayed.setTextSize(18);
            gamesPlayed.setTextColor(Color.BLACK);
            gamesPlayed.setGravity(Gravity.CENTER);
            gamesPlayed.setPadding(8, 8, 8, 8);

            TextView gamesWon = new TextView(this);
            gamesWon.setText(String.valueOf(player.getNumberGamesWon()));
            gamesWon.setTextSize(18);
            gamesWon.setTextColor(Color.BLACK);
            gamesWon.setGravity(Gravity.CENTER);
            gamesWon.setPadding(8, 8, 8, 8);

            row.addView(name);
            row.addView(gamesPlayed);
            row.addView(gamesWon);

            table.addView(row);

            // Divider line
            View divider = new View(this);
            divider.setLayoutParams(new TableRow.LayoutParams(TableRow.LayoutParams.MATCH_PARENT, 2));
            divider.setBackgroundColor(Color.parseColor("#000000"));
            table.addView(divider);
        }

        scrollView.addView(table);
        mainLayout.addView(scrollView);
        setContentView(mainLayout);
    }

    private void showPopupMenu(View view) {
        PopupMenu popupMenu = new PopupMenu(this, view);
        MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.drawer_menu, popupMenu.getMenu());

        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                // Handle the menu item click here, or call onOptionsItemSelected(item)
                return onOptionsItemSelected(item);
            }
        });

        popupMenu.show();
    }



    private List<Player> getPlayers() {
        Scoreboard scoreboard = GameStateManager.loadState(this); // Load the scoreboard
        List<Player> playerList = new ArrayList<>();

        // Loop through all players in the scoreboard and add them to the list
        for (int i = 0; i < scoreboard.getNumberOfPlayers(); i++) {
            playerList.add(scoreboard.getPlayer(i));
        }

        return playerList;
    }

}
