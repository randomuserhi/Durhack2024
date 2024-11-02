namespace Biosphere {
    public class InvalidNameException : Exception {
        public InvalidNameException() : base("Invalid state name! State does not exist in dict!") {
        }
    }

    public class AlreadyExistsException : Exception {
        public AlreadyExistsException() : base() {
        }
    }

    public class AlreadyOccupiedException : Exception {
        public AlreadyOccupiedException() : base("Could not place entity because the position is already occupied!") {
        }
    }

}
