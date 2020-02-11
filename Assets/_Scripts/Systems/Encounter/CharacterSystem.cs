﻿using Components;

public class CharacterSystem {
    private static int name = 1;

    public Character RetrieveCharacter() {
        return new Character(name++, 100, 100, 15, 5, 2, 3);
    }
}