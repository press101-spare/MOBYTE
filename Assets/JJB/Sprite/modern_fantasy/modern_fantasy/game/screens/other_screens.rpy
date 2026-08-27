
## About screen ################################################################
##
## This screen gives credit and copyright information about the game and Ren'Py.
##
## There's nothing special about this screen, and hence it also serves as an
## example of how to make a custom screen.

## Text that is placed on the game's about screen. Place the text between the
## triple-quotes, and leave a blank line between paragraphs.

define gui.about = _p("""
EasyRenPyGui is made by {a=https://github.com/shawna-p}Feniks{/a} {a=https://feniksdev.com/}@feniksdev.com{/a}
""")


screen about():

    tag menu

    use game_menu(_("About"))

    viewport:
        xsize config.screen_width-550
        ysize config.screen_height-350
        align (0.5, 0.5) offset(150,0)

        style_prefix "about"

        mousewheel True pagekeys True
        scrollbars "vertical"

        has vbox
        

        label "[config.name!t]"
        text _("Version [config.version!t]\n")

        if gui.about:
            text "[gui.about!t]\n"

        text _("Made with {a=https://www.renpy.org/}Ren'Py{/a} [renpy.version_only].\n\n[renpy.license!t]")
        


style about_label_text:
    size 36 justify True
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

style about_text:
    color white
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

style about_vscrollbar:
    xoffset 30

## Help screen #################################################################
##
## A screen that gives information about key and mouse bindings. It uses other
## screens (keyboard_help, mouse_help, and gamepad_help) to display the actual
## help.

screen help():

    tag menu

    default device = "keyboard"

    use game_menu(_("Help"))
    style_prefix "help"
    viewport:
        xsize config.screen_width-550
        ysize config.screen_height-350
        align (0.5, 0.5) offset(200,0)
        mousewheel True pagekeys True
        scrollbars "vertical"

        has vbox
        spacing 50

        hbox:
            xfill False xalign 0.5 spacing 20
            textbutton _("Keyboard") action SetScreenVariable("device", "keyboard")
            textbutton _("Mouse") action SetScreenVariable("device", "mouse")

            if GamepadExists():
                textbutton _("Gamepad") action SetScreenVariable("device", "gamepad")

        if device == "keyboard":
            use keyboard_help
        elif device == "mouse":
            use mouse_help
        elif device == "gamepad":
            use gamepad_help


screen keyboard_help():

    frame:
        hbox:
            label _("Enter")
            text _("Advances dialogue and activates the interface.")

    frame:
        hbox:
            label _("Space")
            text _("Advances dialogue without selecting choices.")

    frame:
        hbox:
            label _("Arrow Keys")
            text _("Navigate the interface.")

    frame:
        hbox:
            label _("Escape")
            text _("Accesses the game menu.")

    frame:
        hbox:
            label _("Ctrl")
            text _("Skips dialogue while held down.")

    frame:
        hbox:
            label _("Tab")
            text _("Toggles dialogue skipping.")

    frame:
        hbox:
            label _("Page Up")
            text _("Rolls back to earlier dialogue.")

    frame:
        hbox:
            label _("Page Down")
            text _("Rolls forward to later dialogue.")

    frame:
        hbox:
            label "H"
            text _("Hides the user interface.")

    frame:
        hbox:
            label "S"
            text _("Takes a screenshot.")

    frame:
        hbox:
            label "V"
            text _("Toggles assistive {a=https://www.renpy.org/l/voicing}self-voicing{/a}.")

    frame:
        hbox:
            label "Shift+A"
            text _("Opens the accessibility menu.")


screen mouse_help():

    frame:
        hbox:
            label _("Left Click")
            text _("Advances dialogue and activates the interface.")

    frame:
        hbox:
            label _("Middle Click")
            text _("Hides the user interface.")

    frame:
        hbox:
            label _("Right Click")
            text _("Accesses the game menu.")

    frame:
        hbox:
            label _("Mouse Wheel Up\nClick Rollback Side")
            text _("Rolls back to earlier dialogue.")

    frame:
        hbox:
            label _("Mouse Wheel Down")
            text _("Rolls forward to later dialogue.")


screen gamepad_help():

    frame:
        hbox:
            label _("Right Trigger\nA/Bottom Button")
            text _("Advances dialogue and activates the interface.")

    frame:
        hbox:
            label _("Left Trigger\nLeft Shoulder")
            text _("Rolls back to earlier dialogue.")

    frame:
        hbox:
            label _("Right Shoulder")
            text _("Rolls forward to later dialogue.")

    frame:
        hbox:
            label _("D-Pad, Sticks")
            text _("Navigate the interface.")

    frame:
        hbox:
            label _("Start, Guide, B/Right Button")
            text _("Accesses the game menu.")

    frame:
        hbox:
            label _("Y/Top Button")
            text _("Hides the user interface.")

    textbutton _("Calibrate") action GamepadCalibrate()


style help_vscrollbar:
    xoffset -75
style help_button:
    xmargin 12

style help_label:
    xsize 375
    right_padding 50
    

style help_label_text:
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

style help_text:
    xalign 1.0
    textalign 1.0
    yalign 0.5
    color white
    outlines [ (1, "#2b262609", 0, 0),  (2, "#00000007", 0, 0), (3, "#00000010", 0, 0), (4, "#00000007", 0, 0) ]

style help_frame:
    xsize 1200
    background Frame("gui/history_border.png", 60, 60, 60, 60)
    padding(40, 20)
style help_hbox:
    xfill True

style help_button:
    background Frame("gui/namebox.png", 60, 0, 60, 0, tile=False) xysize(324,98)
    xalign 0.5

style help_button_text:
    align(0.5, 0.5)
    idle_color light_accent
    hover_color white
    selected_color white
