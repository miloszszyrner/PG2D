using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToA
{
    public enum TileProperty
	{
// Warstwa: Bakcground (Nie ma kolizji)

		BACKGROUND,
		BACKGROUND_BROKEN,
		WINDOW_DOWN_LEFT,
		WINDOW_DOWN_RIGHT,
		WINDOW_MIDDLE_LEFT,
		WINDOW_MIDDLE_RIGHT,
		WINDOW_UP_LEFT,
		WINDOW_UP_RIGHT,

		//Warstwa: Decorations (Nie ma kolizji - nakładka na tło)

		EARTH,
		COLUMN_DOWN_LEFT,
		COLUMN_DOWN_RIGHT,
		COLUMN_MIDDLE_DECO_LEFT,
		COLUMN_MIDDLE_DECO_RIGHT,
		COLUMN_MIDDLE_LEFT,
		COLUMN_MIDDLE_RIGHT,
		COLUMN_TOP_LEFT,
		COLUMN_TOP_RIGHT,

		//Warstwa: foreground (są różne kolizje)

		
		TRAP, //(kolizja = RIP)
		FLOOR, //(działa jak podłoże)
		FLOOR_LEFT, //(działa jak podłoże)
		FLOOR_RIGHT, //(działa jak podłoże)
		PLATFORM_CENTER, //(działa jak podłoże)
		PLATFORM_LEFT, //(działa jak podłoże)
		PLATFORM_RIGHT, //(działa jak podłoże)
		STAIRS_PART1, //(przejscie do poziomu)
		STAIRS_PART2, //(przejscie do poziomu)
		STAIRS_PART3, //(przejscie do poziomu) 
		STAIRS_PART4, //(przejscie do poziomu)
		BASE, //(grunt pod podłogą)
		BASE_LEFT, //(ściana, w którą nie możnawejść z lewej strony)
		BASE_RIGHT, //(ściana, w którą nie można wejść z prawej strony)
		BOTTOM, //(sufit)
		BROKEN, //(grunt pod podłogą
	}
}
