using System;

namespace GameDataUI {
	public interface IInputListParent
	{
		void RemoveListElement(IInputListElement child);
	}
}