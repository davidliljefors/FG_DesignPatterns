﻿using System;

public interface ICharacter
{
	int Health { get; set; }
	event Action<int> OnHealthChanged;
}
