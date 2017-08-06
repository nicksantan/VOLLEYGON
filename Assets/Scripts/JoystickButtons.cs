using UnityEngine;

public class JoystickButtons {

    // All input types
    public string horizontal;
    public string vertical;
    public string jump;
    public string grav;
    public string start;

    // Get explicit strings from player number
    public JoystickButtons(int player) {

        switch (player) {

            case 1:
                this.horizontal = (DataManagerScript.xboxMode) ? "Horizontal_P1_Xbox" : "Horizontal_P1";
                this.vertical = (DataManagerScript.xboxMode) ? "Vertical_P1_Xbox" : "Vertical_P1";
                this.jump = (DataManagerScript.xboxMode) ? "Jump_P1_Xbox" : "Jump_P1";
                this.grav = (DataManagerScript.xboxMode) ? "Grav_P1_Xbox" : "Grav_P1";
                this.start = (DataManagerScript.xboxMode) ? "Start_P1_Xbox" : "Start_P1";
                break;

            case 2:
                this.horizontal = (DataManagerScript.xboxMode) ? "Horizontal_P2_Xbox" : "Horizontal_P2";
                this.vertical = (DataManagerScript.xboxMode) ? "Vertical_P2_Xbox" : "Vertical_P2";
                this.jump = (DataManagerScript.xboxMode) ? "Jump_P2_Xbox" : "Jump_P2";
                this.grav = (DataManagerScript.xboxMode) ? "Grav_P2_Xbox" : "Grav_P2";
                this.start = (DataManagerScript.xboxMode) ? "Start_P2_Xbox" : "Start_P2";
                break;

            case 3:
                this.horizontal = (DataManagerScript.xboxMode) ? "Horizontal_P3_Xbox" : "Horizontal_P3";
                this.vertical = (DataManagerScript.xboxMode) ? "Vertical_P3_Xbox" : "Vertical_P3";
                this.jump = (DataManagerScript.xboxMode) ? "Jump_P3_Xbox" : "Jump_P3";
                this.grav = (DataManagerScript.xboxMode) ? "Grav_P3_Xbox" : "Grav_P3";
                this.start = (DataManagerScript.xboxMode) ? "Start_P3_Xbox" : "Start_P3";
                break;

            case 4:
                this.horizontal = (DataManagerScript.xboxMode) ? "Horizontal_P4_Xbox" : "Horizontal_P4";
                this.vertical = (DataManagerScript.xboxMode) ? "Vertical_P4_Xbox" : "Vertical_P4";
                this.jump = (DataManagerScript.xboxMode) ? "Jump_P4_Xbox" : "Jump_P4";
                this.grav = (DataManagerScript.xboxMode) ? "Grav_P4_Xbox" : "Grav_P4";
                this.start = (DataManagerScript.xboxMode) ? "Start_P4_Xbox" : "Start_P4";
                break;
        }
    }
}
