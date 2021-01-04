class ShapeStats {
    public float jump;
    public float speed;
    public float spin;

    public ShapeStats(string type) {

        switch (type) {
            //case "circle":
            //    this.jump = 2200f;
            //    this.speed = 18f;
            //    this.spin = -100f;
            //    break;
            //case "triangle":
            //    this.jump = 2000f;
            //    this.speed = 18f;
            //    this.spin = -200f;
            //    break;
            //case "square":
            //    this.jump = 2000f;
            //    this.speed = 15f;
            //    this.spin = -150f;
            //    break;
            //case "rectangle":
            //    this.jump = 2000f;
            //    this.speed = 13f;
            //    this.spin = -150f;
            //    break;
            //case "star":
            //    this.jump = 2000f;
            //    this.speed = 15f;
            //    this.spin = -100f;
            //    break;
            //case "trapezoid":
            //this.jump = 2000f;
            //this.speed = 15f;
            //this.spin = -150f;
            //break;

            case "circle":
                this.jump = 2000f;
                this.speed = 18f;
                this.spin = -100f;
                break;
            case "triangle":
                this.jump = 2000f;
                this.speed = 18f;
                this.spin = -200f;
                break;
            case "square":
                this.jump = 2000f;
                this.speed = 18f;
                this.spin = -150f;
                break;
            case "rectangle":
                this.jump = 2000f;
                this.speed = 18f;
                this.spin = -150f;
                break;
            case "star":
                this.jump = 2000f;
                this.speed = 18f;
                this.spin = -100f;
                break;
            case "trapezoid":
                this.jump = 2000f;
                this.speed = 18f;
                this.spin = -150f;
            break;
        }
    }
}