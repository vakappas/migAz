using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;

namespace MigAz.AWS.MigrationSource
{
    public class LoadBalancer : ILoadBalancer
    {
        private Amazon.ElasticLoadBalancing.Model.LoadBalancerDescription _LoadBalancerSource;

        private LoadBalancer() { }

        public LoadBalancer(Amazon.ElasticLoadBalancing.Model.LoadBalancerDescription loadBalancerSource)
        {
            this._LoadBalancerSource = loadBalancerSource;
        }

        
        public string Name
        {
            get { return _LoadBalancerSource.LoadBalancerName; }
        }
    }
}