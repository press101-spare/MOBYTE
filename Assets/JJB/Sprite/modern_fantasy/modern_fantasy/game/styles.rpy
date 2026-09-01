################################################################################
## Initialization
################################################################################

## The init offset statement causes the initialization statements in this file
## to run before init statements in any other file.
init offset = -2

## Calling gui.init resets the styles to sensible default values, and sets the
## width and height of the game.
init python:
    gui.init(1920, 1080)

define config.check_conflicting_properties = True

###Colours


define light_accent = u"#A3886F"
define white = u"#C5C0BF" ###purposefully little off-white
define dark_accent = u"#3D342C"
################################################################################
## GUI Configuration Variables
################################################################################
## Some choice gui values have been left in, to make them
## easier to adjust for accessibility purposes e.g. to allow
## players to change the default text font or size by rebuilding the gui.
## You may add more back if you need to adjust them, or find-and-replace
## any instances where they are used directly with their value.

# The text font for dialogue and choice menus
define gui.text_font = gui.preference("font", "fonts/CrimsonText-Regular.ttf")
# The text font for buttons
define gui.interface_text_font = gui.preference("interface_font", "fonts/CrimsonText-Regular.ttf")
# The default size of in-game text
define gui.text_size = gui.preference("size", 31)
# The font for character names
define gui.name_text_font = gui.preference("name_font", "fonts/CrimsonText-Regular.ttf")
# The size for character names
define gui.name_text_size = gui.preference("name_size", 43)

## Localization ################################################################

## This controls where a line break is permitted. The default is suitable
## for most languages. A list of available values can be found at
## https://www.renpy.org/doc/html/style_properties.html#style-property-language

define gui.language = "unicode"


################################################################################
## Style Initialization
################################################################################

init offset = -1

################################################################################
## Styles
################################################################################

style default:
    font gui.text_font
    size gui.text_size
    language gui.language

style input:
    adjust_spacing False

style hyperlink_text:
    hover_underline True
    color light_accent

style gui_text:
    color '#ffffff'
    size gui.text_size
    font gui.interface_text_font

style button:
    xysize (None, None)
    padding (0, 0)

style button_text:
    is gui_text
    yalign 0.5
    xalign 0.0
    ## The color used for a text button when it is neither selected nor hovered.
    idle_color light_accent
    ## The color that is used for buttons and bars that are hovered.
    hover_color white
    ## The color used for a text button when it is selected but not focused. A
    ## button is selected if it is the current screen or preference value.
    selected_color '#ffffff'
    ## The color used for a text button when it cannot be selected.
    insensitive_color '#8888887f'

style label_text:
    is gui_text
    size 36
    color light_accent

style vscrollbar:
    xsize 44
    base_bar None
    thumb Frame("gui/scrollbar.png", 0, 20, 0, 20, tile=False)
    unscrollable 'hide'

style slider:
    ysize 91 xsize 811
    base_bar "gui/options/bar_base.png"
    thumb "gui/options/bar_thumb.png"
    thumb_offset 46  left_gutter 35 right_gutter 35




