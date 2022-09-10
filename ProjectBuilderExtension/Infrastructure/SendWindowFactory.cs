using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBuilderExtension.Infrastructure
{
	public static class SendingWindowFactory
	{
		public static SendingSettingWindow Build(SendingSettingWindowOptions options)
		{
			return new SendingSettingWindow(options);
		}
	}
}
