using BuildApp.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuildApp.Models
{
    public class Status
    {
        int requestSerialNum;
        string receiverUserName;
        int addressId;
        int requestStatus;

        public int RequestSerialNum { get => requestSerialNum; set => requestSerialNum = value; }
        public string ReceiverUserName { get => receiverUserName; set => receiverUserName = value; }
        public int AddressId { get => addressId; set => addressId = value; }
        public int RequestStatus { get => requestStatus; set => requestStatus = value; }

        public Status(int requestSerialNum, string receiverUserName, int addressId, int requestStatus)
        {
            RequestSerialNum = requestSerialNum;
            ReceiverUserName = receiverUserName;
            AddressId = addressId;
            RequestStatus = requestStatus;
        }
        public Status()
        {

        }
        public List<List<List<Status>>> GetRequestStatusBuildingByRequestId(int requestId)
        {
            DBservices db = new DBservices();
           
            return db.GetRequestBuildingByRequestId(requestId);
        }
        public string SawAllNotification(string un)
        {
            DBservices db = new DBservices();
            return db.SawAllNotification(un);
        }
        public int GetNumOfNotification(string un)
        {
            DBservices db = new DBservices();
            return db.GetNumOfNotification(un);
        }
        public string AcceptRequest(string un, int serialNum)
        {
            DBservices db = new DBservices();
            db.AcceptRequest(un, serialNum);
            Request r = db.GetRequestDetails(serialNum);
            string token = db.GetToken(r.FromUserName);
            UserToShow u = db.GetUserToShow(un);
            PushNotData pnd = new PushNotData(
                    to: token,
                    title: "Request accepted!",
                    body: $"{u.FirstName} {u.LastName} accept your {r.Type} request",
                    badge: 0,
                    data: new Data(serialNum,"acceptRequest")
                    );
            
            pnd.SendPushNotification();
            return "ok";
        }
    }
}