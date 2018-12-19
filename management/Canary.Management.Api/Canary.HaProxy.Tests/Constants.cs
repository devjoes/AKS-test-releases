using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.HaProxy.Tests
{
    public class Constants
    {
        public class TwoConditionalRoutesWithSSl
        {
            public const string ShowAclResponse =
                "# id (file) description\n" +
                "                       0 () acl 'req.ssl_hello_type' file '/etc/haproxy/haproxy.cfg' line 60\n" +
                "            1 () acl 'ssl_fc' file '/etc/haproxy/haproxy.cfg' line 77\n" +
                "                                                                     2 () acl 'ssl_fc' file '/etc/haproxy/haproxy.cfg' line 78\n" +
                "                                              3 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 79\n" +
                "                    4 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 92\n" +
                "                                                                          5 () acl 'req.cook' file '/etc/haproxy/haproxy.cfg' line 92\n" +
                "                                                     6 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 93\n" +
                "                           7 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 94\n" +
                " 8 () acl 'req.cook' file '/etc/haproxy/haproxy.cfg' line 94\n" +
                "                                                            9 () acl 'ssl_fc' file '/etc/haproxy/haproxy.cfg' line 108\n" +
                "                                      10 () acl 'ssl_fc' file '/etc/haproxy/haproxy.cfg' line 109\n";

            public const string ShowAcl3Response = "0x55c37246e680 foo.bar\n" +
                                                   "                      0x55c37246e760 foo.bar:80\n" +
                                                   "                                               0x55c37246e860 foo.bar:443";

            public const string ShowAcl4Response =
                "0x56034dbb41c0 /app1\n" +
                "                   0x56034dbb4280 ||\n" +
                "                                    0x56034dbb4340 var(txn.path)\n" +
                "                                                                0x56034dbb4400 -m\n" +
                "                                                                                 0x56034dbb44c0 beg\n" +
                "                                                                                                   0x56034dbb4580 /app1_conditional";

            public const string ShowAcl5Response =
                "                                0x55c372478800 foo\n" +
                "                                                  0x55c3724d9be0 bar";

            public const string ShowAcl7Response =
                "0x56034dbb41c0 /\n" +
                "                   0x56034dbb4280 ||\n" +
                "                                    0x56034dbb4340 var(txn.path)\n" +
                "                                                                0x56034dbb4400 -m\n" +
                "                                                                                 0x56034dbb44c0 beg\n" +
                "                                                                                                   0x56034dbb4580 /_conditional";

            public const string ShowAcl8Response =
                "                                                                                                0x55c3724798c0 foo";
        }

        public class ThreeConditionalRoutesWithSSl
        {
            public const string ShowAclResponse =
"# id (file) description\n" +
                "                       0 () acl 'req.ssl_hello_type' file '/etc/haproxy/haproxy.cfg' line 75\n" +
                "                                                                                            1 () acl 'ssl_fc' file '/etc/haproxy/haproxy.cfg' line 93\n" +
                "                                                                                                                                                     2 () acl 'ssl_fc' file '/etc/haproxy/haproxy.cfg' line 94\n" +
                "                                                 3 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 95\n" +
                "                                                                                                       4 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 108\n" +
                " 5 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 109\n" +
                "                                                        6 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 110\n" +
                "                                                                                                               7 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 111\n" +
                "         8 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 112\n" +
                "                                                                9 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 114\n" +
                "                                                                                                                       10 () acl 'req.body_size' file '/etc/haproxy/haproxy.cfg' line 114\n" +
                "                            11 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 115\n" +
                "                                                                                    12 () acl 'req.body_size' file '/etc/haproxy/haproxy.cfg' line 115\n" +
                "                                                                                                                                                      13 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 116\n" +
                "                                                 14 () acl 'req.body_size' file '/etc/haproxy/haproxy.cfg' line 116\n" +
                "                                                                                                                   15 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 117\n" +
                "              16 () acl 'req.body_size' file '/etc/haproxy/haproxy.cfg' line 117\n" +
                "                                                                                17 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 118\n" +
                "                                                                                                                                        18 () acl 'req.body_size' file '/etc/haproxy/haproxy.cfg' line 118\n" +
                "                                             19 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 120\n" +
                "                                                                                                     20 () acl 'req.cook' file '/etc/haproxy/haproxy.cfg' line 120\n" +
                "     21 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 121\n" +
                "                                                             22 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 122\n" +
                "                                                                                                                     23 () acl 'req.cook' file '/etc/haproxy/haproxy.cfg' line 122\n" +
                "                     24 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 123\n" +
                "                                                                             25 () acl 'req.cook' file '/etc/haproxy/haproxy.cfg' line 123\n" +
                "                                                                                                                                          26 () acl 'var' file '/etc/haproxy/haproxy.cfg' line 136";

            private const string ShowAcl3Response =
                "0x556639499840 activitymanager-demo.retail-azure.js-devops.co.uk\n" +
                "                                                                0x5566394999c0 activitymanager-demo.retail-azure.js-devops.co.uk:80\n" +
                "                                                                                                                                   0x556639491760 activitymanager-demo.retail-azure.js-devops.co.uk:443\n";
            private const string ShowAcl4Response =
                "                                          0x55663949a800 /api_conditional\n";
            private const string ShowAcl5Response =
                "                                                                         0x55663949af60 /api\n";
            private const string ShowAcl6Response =
                "                                                                                            0x55663949b660 /_orangesnowconditional\n";
            private const string ShowAcl7Response =
                "                                                                                                                                  0x55663949be00 /_bluesnowconditional\n";

            private const string ShowAcl8Response =
                "         0x55663949c5e0 /\n";
            private const string ShowAcl9Response =
                "                         0x55663949cee0 /api_conditional\n";
            private const string ShowAcl10Response =
                "                                                        0x55663949d300 209715201:\n";
            private const string ShowAcl11Response =
                "                                                                                 0x55663949d900 /api\n";
            private const string ShowAcl12Response =
                "                                                                                                    0x55663949dd00 209715201:\n";
            private const string ShowAcl13Response =
                "                                                                                                                             0x55663949e300 /_orangesnowconditional\n";
            private const string ShowAcl14Response =
                "      0x55663949e720 209715201:\n";
            private const string ShowAcl15Response =
                "                               0x55663949ed20 /_bluesnowconditional\n";
            private const string ShowAcl16Response =
                "                                                                   0x55663949f140 209715201:\n";
            private const string ShowAcl17Response =
                "                                                                                            0x55663949f740 /\n";
            private const string ShowAcl18Response =
                "                                                                                                            0x55663949fb40 209715201:\n";
            private const string ShowAcl19Response =
                "                                                                                                                                     0x5566394a0420 /api\n" +
                "                                                                                                                                                        0x5566394a04e0 ||\n" +
                "            0x5566394a05c0 var(txn.path)\n" +
                "                                        0x5566394a06a0 -m\n" +
                "                                                         0x5566394a0780 beg\n" +
                "                                                                           0x5566394a0860 /api_conditional\n";
            private const string ShowAcl20Response =
                "                                                                                                          0x5566394a0cc0 p382135a0-6e5f-454d-aaa7-7e8b12f39836\n";

            private const string ShowAcl21Response =
                " 0x5566394a1300 /api\n";
            private const string ShowAcl22Response =
                "                    0x5566394a1960 /\n                                    0x5566394a1a20 ||\n" +
                "                                                     0x5566394a1b00 var(txn.path)\n" +
                "                                                                                 0x5566394a1be0 -m\n" +
                "                                                                                                  0x5566394a1cc0 beg\n" +
                "                                                                                                                    0x5566394a1da0 /_orangesnowconditional\n";
            private const string ShowAcl23Response =
                "                                                                                                                                                          0x5566394facc0 _s0119_\n" +
                "                   0x556639505700 p7147e3cb-6cd4-4754-87fb-e4f53dd0c6d5\n";
            private const string ShowAcl24Response =
                "                                                                       0x5566394a28a0 /\n" +
                "                                                                                       0x5566394a2960 ||\n" +
                "                                                                                                        0x5566394a2a40 var(txn.path)\n" +
                "                                                                                                                                    0x5566394a2b20 -m\n" +
                "                                                                                                                                                     0x5566394a2c00 beg\n" +
                "          0x5566394a2ce0 /_bluesnowconditional\n";
            private const string ShowAcl25Response =
                "                                              0x556639505d00 p0675d292-6803-4cf2-8972-ff7773b227d6\n";
            private const string ShowAcl26Response =
                "                                                                                                  0x5566394a7d60 https\n";
        }

        public const string Stats =
            "# pxname,svname,qcur,qmax,scur,smax,slim,stot,bin,bout,dreq,dresp,ereq,econ,eresp,wretr,wredis,status,weight,act,bck,chkfail,chkdown,lastchg,downtime,qlimit,pid,iid,sid,throttle,lbtot,tracked,type,rate,rate_lim,rate_max,check_status,check_code,check_duration,hrsp_1xx,hrsp_2xx,hrsp_3xx,hrsp_4xx,hrsp_5xx,hrsp_other,hanafail,req_rate,req_rate_max,req_tot,cli_abrt,srv_abrt,comp_in,comp_out,comp_byp,comp_rsp,lastsess,last_chk,last_agt,qtime,ctime,rtime,ttime,agent_status,agent_code,agent_duration,check_desc,agent_desc,check_rise,check_fall,check_health,agent_rise,agent_fall,agent_health,addr,cookie,mode,algo,conn_rate,conn_rate_max,conn_tot,intercepted,dcon,dses,\n" +
            "                                      upstream-default-backend,server0000,0,0,0,1,,1,218,145,,0,,0,0,0,0,UP,1,1,0,0,0,5413,0,,1,2,1,,1,,2,0,,1,L4OK,,0,0,0,0,1,0,0,,,,,0,0,,,,,5217,,,0,0,0,0,,,,Layer4 check passed,,2,3,4,,,,10.244.4.11:8080,,http,,,,,,,,\n" +
            "                                                                                                upstream-default-backend,server0001,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,2,2,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                                                                                                                           upstream-default-backend,server0002,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,2,3,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                                                                                                                                                      upstream-default-backend,server0003,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,2,4,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                    upstream-default-backend,BACKEND,0,0,0,1,200,1,218,145,0,0,,0,0,0,0,UP,1,1,0,,0,5413,0,,1,2,0,,1,,1,0,,1,,,,0,0,0,1,0,0,,,,1,0,0,0,0,0,0,5217,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                  joe-test-http-svc-8080,server0000,0,0,0,0,,0,0,0,,0,,0,0,0,0,UP,1,1,0,0,0,5413,0,,1,3,1,,0,,2,0,,0,L4OK,,0,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,Layer4 check passed,,2,3,4,,,,10.244.5.30:8080,,http,,,,,,,,\n" +
            "                                                                                                    joe-test-http-svc-8080,server0001,0,0,0,0,,0,0,0,,0,,0,0,0,0,UP,1,1,0,0,0,5413,0,,1,3,2,,0,,2,0,,0,L4OK,,1,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,Layer4 check passed,,2,3,4,,,,10.244.3.10:8080,,http,,,,,,,,\n" +
            "                                                                                                                                                      joe-test-http-svc-8080,server0002,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,3,3,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                  joe-test-http-svc-8080,server0003,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,3,4,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                                           joe-test-http-svc-8080,BACKEND,0,0,0,0,200,0,0,0,0,0,,0,0,0,0,UP,2,2,0,,0,5413,0,,1,3,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                 joe-test-ingress-default-backend-8080,server0000,0,0,0,0,,0,0,0,,0,,0,0,0,0,UP,1,1,0,0,0,5413,0,,1,4,1,,0,,2,0,,0,L4OK,,0,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,Layer4 check passed,,2,3,4,,,,10.244.4.11:8080,,http,,,,,,,,\n" +
            "                                                                                                                                  joe-test-ingress-default-backend-8080,server0001,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,4,2,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "             joe-test-ingress-default-backend-8080,server0002,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,4,3,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                                                     joe-test-ingress-default-backend-8080,server0003,0,0,0,0,,0,0,0,,0,,0,0,0,0,MAINT,1,1,0,0,0,5413,5413,,1,4,4,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,127.0.0.1:81,,http,,,,,,,,\n" +
            "                                                                                             joe-test-ingress-default-backend-8080,BACKEND,0,0,0,0,200,0,0,0,0,0,,0,0,0,0,UP,1,1,0,,0,5413,0,,1,4,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                                                  httpsfront,FRONTEND,,,0,0,2000,0,0,0,0,0,0,,,,,OPEN,,,,,,,,,1,5,0,,,,0,0,0,0,,,,,,,,,,,0,0,0,,,0,0,0,0,,,,,,,,,,,,,,,,,,,,,tcp,,0,0,0,,0,0,\n" +
            "                                                                                                                httpsback-shared-backend,shared-https-frontend,0,0,0,0,,0,0,0,,0,,0,0,0,0,no check,1,1,0,,,5413,,,1,6,1,,0,,2,0,,0,,,,,,,,,,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,unix,,tcp,,,,,,,,\n" +
            "                                                                                                                                    httpsback-shared-backend,BACKEND,0,0,0,0,200,0,0,0,0,0,,0,0,0,0,UP,1,1,0,,0,5413,0,,1,6,0,,0,,1,0,,0,,,,,,,,,,,,,,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,tcp,roundrobin,,,,,,,\n" +
            "                                                                                                                                                    httpback-shared-backend,shared-http-frontend,0,0,0,0,,0,0,0,,0,,0,0,0,0,no check,1,1,0,,,5413,,,1,7,1,,0,,2,0,,0,,,,0,0,0,0,0,0,,,,,0,0,,,,,-1,,,0,0,0,0,,,,,,,,,,,,unix,,http,,,,,,,,\n" +
            "                httpback-shared-backend,BACKEND,0,0,0,0,1,0,0,0,0,0,,0,0,0,0,UP,1,1,0,,0,5413,0,,1,7,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                     httpfront-shared-frontend,FRONTEND,,,0,1,2000,1,185,145,0,0,0,,,,,OPEN,,,,,,,,,1,8,0,,,,0,0,0,1,,,,0,0,0,1,0,0,,0,1,1,,,0,0,0,0,,,,,,,,,,,,,,,,,,,,,http,,0,1,1,0,0,0,\n" +
            "                                              httpback-default-backend,shared-http-frontend,0,0,0,1,,1,185,145,,0,,0,0,0,0,no check,1,1,0,,,5413,,,1,9,1,,1,,2,0,,1,,,,0,0,0,1,0,0,,,,,0,0,,,,,5217,,,0,0,0,0,,,,,,,,,,,,unix,,http,,,,,,,,\n" +
            "                                                                              httpback-default-backend,BACKEND,0,0,0,1,200,1,185,145,0,0,,0,0,0,0,UP,1,1,0,,0,5413,0,,1,9,0,,1,,1,0,,1,,,,0,0,0,1,0,0,,,,1,0,0,0,0,0,0,5217,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                            httpfront-default-backend,FRONTEND,,,0,1,2000,1,218,145,0,0,0,,,,,OPEN,,,,,,,,,1,10,0,,,,0,0,0,1,,,,0,0,0,1,0,0,,0,1,1,,,0,0,0,0,,,,,,,,,,,,,,,,,,,,,http,,0,1,1,0,0,0,\n" +
            "                                                                                                                      error413,BACKEND,0,0,0,0,1,0,0,0,0,0,,0,0,0,0,UP,0,0,0,,0,5413,,,1,11,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                                            error495,BACKEND,0,0,0,0,1,0,0,0,0,0,,0,0,0,0,UP,0,0,0,,0,5413,,,1,12,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                                                  error496,BACKEND,0,0,0,0,1,0,0,0,0,0,,0,0,0,0,UP,0,0,0,,0,5413,,,1,13,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                                                        error503noendpoints,FRONTEND,,,0,0,2000,0,0,0,0,0,0,,,,,OPEN,,,,,,,,,1,14,0,,,,0,0,0,0,,,,0,0,0,0,0,0,,0,0,0,,,0,0,0,0,,,,,,,,,,,,,,,,,,,,,http,,0,0,0,0,0,0,\n" +
            "                                                                                                                                        error503noendpoints,BACKEND,0,0,0,0,200,0,0,0,0,0,,0,0,0,0,UP,0,0,0,,0,5413,,,1,14,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                                                                           stats,FRONTEND,,,0,0,2000,0,0,0,0,0,0,,,,,OPEN,,,,,,,,,1,15,0,,,,0,0,0,0,,,,0,0,0,0,0,0,,0,0,0,,,0,0,0,0,,,,,,,,,,,,,,,,,,,,,http,,0,0,0,0,0,0,\n" +
            "                                                                                                                                             stats,BACKEND,0,0,0,0,200,0,0,0,0,0,,0,0,0,0,UP,0,0,0,,0,5413,,,1,15,0,,0,,1,0,,0,,,,0,0,0,0,0,0,,,,0,0,0,0,0,0,0,-1,,,0,0,0,0,,,,,,,,,,,,,,http,roundrobin,,,,,,,\n" +
            "                                                                                                                                                  healthz,FRONTEND,,,0,0,2000,0,0,0,0,0,0,,,,,OPEN,,,,,,,,,1,16,0,,,,0,0,0,0,,,,0,0,0,0,0,0,,0,0,0,,,0,0,0,0,,,,,,,,,,,,,,,,,,,,,http,,0,0,0,0,0,0,";
    }
}
