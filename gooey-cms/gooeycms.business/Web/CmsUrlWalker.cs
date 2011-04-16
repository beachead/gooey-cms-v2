using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Web
{
    /// <summary>
    /// This class can be used to walk up and down a cms url.
    /// </summary>
    public class CmsUrlWalker
    {
        CmsUrl url = null;

        IList<String> pieces = null;
        int currentPosition = 0;

        public CmsUrlWalker(CmsUrl url)
        {
            this.url = url;
            
            String path = this.url.Path;
            if (path.StartsWith("/"))
                path = path.Substring(1);

            String[] arr = path.Split('/');
            pieces = new List<String>(arr);

            currentPosition = 0;
        }

        public int Depth
        {
            get { return currentPosition + 1; }
        }

        public void MoveFirst()
        {
            currentPosition = 1;
        }

        public void MoveLast()
        {
            currentPosition = pieces.Count;
        }

        public bool Next()
        {
            bool result = true;

            if (currentPosition < pieces.Count)
                currentPosition++;
            else
                result = false;

            return result;
        }

        public bool Previous()
        {
            bool result = true;
            if (currentPosition > 1)
                currentPosition--;
            else
                result = false;

            return result;
        }

        public String GetIndividualPath()
        {
            return pieces[currentPosition - 1];
        }

        public String GetParentPath()
        {
            if (currentPosition == 1)
                return "/";


            currentPosition--;
            String parent = GetCurrentPath();
            currentPosition++;

            return parent;
        }

        public String GetCurrentPath()
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < currentPosition; i++)
            {
                result.Append(pieces[i]).Append("/");
            }

            result.Remove(result.Length - 1, 1);

            return result.ToString();
        }

        public Boolean IsLast
        {
            get { return this.currentPosition == pieces.Count; }
        }
    }
}
