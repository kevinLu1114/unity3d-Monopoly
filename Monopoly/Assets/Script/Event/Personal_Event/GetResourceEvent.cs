﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    class GetResourceEvent : EventBase
    {
        public GetResourceEvent(string n,bool g,int w, string d) :base(n,g,w,d)
        {

        }
        public override void DoEvent(List<Group> droup_list, Group group)
        {
        //所有資源增加5%
        group.Resource.mineral = Convert.ToInt32(group.Resource.mineral * 1.05);

        }
    }
