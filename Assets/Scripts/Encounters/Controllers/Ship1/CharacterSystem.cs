using Views;

public class CharacterSystem {
    private static int name = 1;

    public Character RetrieveCharacter() {
        return new Character("Unit " + name++, Team.Player,  100, 100, 15, 5, 2, 3);
    }
}