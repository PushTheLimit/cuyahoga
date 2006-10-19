using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.RemoteContent
{
	/// <summary>
	/// Summary description for EditFeed.
	/// </summary>
	public class EditFeed : ModuleAdminBasePage
	{
		private Feed _feed;
		private RemoteContentModule _remoteContentModule;

		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.WebControls.TextBox txtUrl;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.TextBox txtNumberOfItems;
		protected System.Web.UI.WebControls.Label lblPubDate;
		protected System.Web.UI.WebControls.Label lblUpdateTimestamp;
		protected System.Web.UI.WebControls.Button btnVerify;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvUrl;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvNumberOfItems;
		protected System.Web.UI.WebControls.CompareValidator covNumberOfItems;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._remoteContentModule = base.Module as RemoteContentModule;
			this.btnCancel.Attributes.Add("onclick", String.Format("document.location.href='AdminRemoteContent.aspx{0}'", base.GetBaseQueryString()));

			if (Request.QueryString["FeedId"] != null)
			{
				int feedId = Int32.Parse(Request.QueryString["FeedId"]);
				if (feedId > 0)
				{
					this._feed = this._remoteContentModule.GetFeedById(feedId);
					if (! this.IsPostBack)
					{
						BindFeed();
					}
					this.btnSave.Enabled = true;
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onclick", "return confirm('Are you sure?');");
				}
				else
				{
					this._feed = new Feed();
					this.btnSave.Enabled = false;
				}
			}
		}

		private void BindFeed()
		{
			this.txtUrl.Text = this._feed.Url;
			this.txtTitle.Text = this._feed.Title;
			this.txtNumberOfItems.Text = this._feed.NumberOfItems.ToString();
			this.lblPubDate.Text = this._feed.PubDate != DateTime.MinValue ? this._feed.PubDate.ToString() : "";
			this.lblUpdateTimestamp.Text = this._feed.UpdateTimestamp != DateTime.MinValue ? this._feed.UpdateTimestamp.ToString() : "";
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnVerify_Click(object sender, System.EventArgs e)
		{
			try
			{
				this._feed = this._remoteContentModule.VerifyFeed(this.txtUrl.Text);
				BindFeed();
				this.btnSave.Enabled = true;
				base.ShowMessage("Feed is valid");
			}
			catch (Exception ex)
			{
				this.btnSave.Enabled = false;
				base.ShowError(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this._feed.Url = this.txtUrl.Text;
			this._feed.Title = this.txtTitle.Text;
			this._feed.NumberOfItems = Int32.Parse(this.txtNumberOfItems.Text);
			this._feed.Section = base.Section;
			try
			{
				this._remoteContentModule.SaveFeed(this._feed);
				Context.Response.Redirect("AdminRemoteContent.aspx" + base.GetBaseQueryString());
			}
			catch (Exception ex)
			{
				base.ShowError(ex.Message);
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				this._remoteContentModule.DeleteFeed(this._feed);
				Context.Response.Redirect("AdminRemoteContent.aspx" + base.GetBaseQueryString());
			}
			catch (Exception ex)
			{
				base.ShowError(ex.Message);
			}
		}
	}
}
