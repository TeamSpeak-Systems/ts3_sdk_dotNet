using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace iOS2
{
	public class LogSource : UITableViewSource
	{
		readonly List<string> Items;
		const string CellIdentifier = "TableCell";

		public LogSource(List<string> items)
		{
			Items = items;
		}

		public void Append(string message)
		{
			Items.Add(message);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier)
				?? new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
			cell.TextLabel.Text = Items[indexPath.Row];
			cell.TextLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
			cell.TextLabel.Lines = 0;
			return cell;
		}
	}
}
