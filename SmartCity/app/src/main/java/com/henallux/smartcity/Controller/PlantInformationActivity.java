package com.henallux.smartcity.Controller;

import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ImageView;
import android.widget.TextView;

import com.henallux.smartcity.Model.Plant;
import com.henallux.smartcity.R;
import com.squareup.picasso.Picasso;

public class PlantInformationActivity extends AppCompatActivity {

    private Boolean login;
    private SharedPreferences preferences;
    private SharedPreferences.Editor editor;
    ImageView picture;
    TextView namePlant, descr;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_plant_information);

        preferences = PreferenceManager.getDefaultSharedPreferences(this);
        login = preferences.getBoolean("login", false);
        editor=preferences.edit();

        Bundle bundle = this.getIntent().getExtras();
        Plant plant = (Plant)bundle.getSerializable("plant");

        picture = (ImageView) findViewById(R.id.picturePlant);
        Picasso
                .with(this)
                .load("http://res.cloudinary.com/vnckcloud/image/upload/v1513530594/Graines-de-tournesol_nzp4gx.jpg")
                .into(picture);

        namePlant = (TextView) findViewById(R.id.namePlant);
        namePlant.setText(plant.getName());

        descr = (TextView) findViewById(R.id.descriptionPlant);
        descr.setText(plant.getDescription());
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        if (login)
            getMenuInflater().inflate(R.menu.menu_main_sign_out, menu);
        else
            getMenuInflater().inflate(R.menu.menu_main_sign_in, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item){

        switch (item.getItemId())
        {
            case R.id.profile:
                startActivity(new Intent(PlantInformationActivity.this, UserProfileActivity.class));
                return true;
            case R.id.sign_in:
                startActivity(new Intent(PlantInformationActivity.this, LoginActivity.class));
                return true;
            case R.id.sign_out:
                editor.putString("token", "");
                editor.commit();
                startActivity(new Intent(PlantInformationActivity.this, MainActivity.class));
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }
}