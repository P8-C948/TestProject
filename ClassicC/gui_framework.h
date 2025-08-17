#ifndef GUI_FRAMEWORK_H
#define GUI_FRAMEWORK_H

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Basic widget types
typedef enum {
    WIDGET_BUTTON,
    WIDGET_LABEL,
    WIDGET_TEXTBOX
} WidgetType;

// Widget structure
typedef struct Widget {
    int x, y, width, height;
    WidgetType type;
    char text[256];
    void (*onClick)(struct Widget*);
    struct Widget* next;
} Widget;

// Window structure
typedef struct {
    int width, height;
    char title[100];
    Widget* widgets;
} Window;

// Function declarations
Window* createWindow(int width, int height, const char* title);
Widget* createButton(int x, int y, int width, int height, const char* text);
Widget* createLabel(int x, int y, const char* text);
void addWidget(Window* window, Widget* widget);
void renderWindow(Window* window);
void handleInput(Window* window);

#endif