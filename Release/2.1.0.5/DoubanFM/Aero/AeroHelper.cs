using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using DoubanFM.Interop;

namespace DoubanFM.Aero
{
	/// <summary>
	/// Ϊ��������AeroЧ���ṩ֧��
	/// </summary>
	public class AeroHelper
	{
		/// <summary>
		/// �����ڱ߿��AeroЧ����չ���ͻ�����
		/// </summary>
		/// <param name="window">Ŀ�괰��</param>
		/// <param name="margin">��߾�</param>
		/// <returns>�ɹ����</returns>
		public static bool ExtendGlassFrame(Window window, Thickness margin)
		{
			if (!AeroGlassCompositionEnabled)
				return false;

			IntPtr hwnd = new WindowInteropHelper(window).Handle;
			if (hwnd == IntPtr.Zero)
				throw new InvalidOperationException("������AeroЧ��ǰ���ڱ�������ʾ");

			MARGINS margins = new MARGINS((int)margin.Left, (int)margin.Top, (int)margin.Right, (int)margin.Bottom);
			NativeMethods.DwmExtendFrameIntoClientArea(hwnd, ref margins);

			return true;
		}

		/// <summary>
		/// �ڴ��ڱ�������ģ��Ч��
		/// </summary>
		/// <param name="window">Ŀ�괰��</param>
		/// <returns>�ɹ����</returns>
		public static bool EnableBlurBehindWindow(Window window)
		{
			if (!AeroGlassCompositionEnabled)
				return false;

			IntPtr hwnd = new WindowInteropHelper(window).Handle;
			if (hwnd == IntPtr.Zero)
				throw new InvalidOperationException("������AeroЧ��ǰ���ڱ�������ʾ");

			//����DWM_BLURBEHIND�ṹ
			DWM_BLURBEHIND bb = new DWM_BLURBEHIND();
			bb.dwFlags = DWM_BLURBEHIND.DWM_BB_ENABLE | DWM_BLURBEHIND.DWM_BB_BLURREGION;
			bb.fEnable = true;
			bb.hRegionBlur = NativeMethods.CreateRectRgn(0, 0, (int)window.ActualWidth, (int)window.ActualHeight);

			try
			{
				NativeMethods.DwmEnableBlurBehindWindow(hwnd, ref bb);
			}
			catch { }
			//���վ��
			NativeMethods.DeleteObject(bb.hRegionBlur);
			
			return true;
		}

		/// <summary>
		/// ϵͳ�Ƿ�������Aero��Ч��ϣ���û���ã�����ʹ��AeroЧ��
		/// </summary>
		public static bool AeroGlassCompositionEnabled
		{
			get
			{
				//ֻ��Windows Vista����߰汾��֧��Aero��Ч
				if (Environment.OSVersion.Version.Major >= 6)
				{
					try
					{
						return NativeMethods.DwmIsCompositionEnabled();
					}
					catch
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
		}
	}
}