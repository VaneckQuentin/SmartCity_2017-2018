package com.henallux.smartcity;

import android.content.Context;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.henallux.smartcity.Model.Garden;

import java.util.ArrayList;

public class Custom_Gardens_Adapter extends ArrayAdapter<Garden> {
    public Custom_Gardens_Adapter(@NonNull Context context, @NonNull ArrayList<Garden> list_home_menu) {
        super(context, R.layout.list_view_home_menu, list_home_menu);
    }

    @Override
    public View getView(int position, View view, ViewGroup viewGroup) {
        LayoutInflater layout = LayoutInflater.from(getContext());
        View shapeOfList = layout.inflate(R.layout.list_view_gardens_listing, viewGroup, false);
        Garden line_menu = getItem(position);
        TextView nameChoice = (TextView) shapeOfList.findViewById(R.id.choiceGardenListView);
        nameChoice.setText(line_menu.getName());
        return shapeOfList;
    }
}