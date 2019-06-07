using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest.Models
{
    public class UserInfo
    {
        public string Uid { get; set; }
        public string UsersName { get; set; }
        public string FullName { get; set; }
        public string ParentId { get; set; }
        public string Email { get; set; }
        public string Htmleditormode { get; set; }
        public string Templateeditormode { get; set; }
        public string Questionselectormode { get; set; }
        public object OneTimePw { get; set; }
        public string Dateformat { get; set; }
        public string Created { get; set; }
        public object Modified { get; set; }
        public string Lang { get; set; }
        public IEnumerable<Claim> Permissions { get; set; }
    }

    public class Claim
    {
        public string Id { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Uid { get; set; }
        public string Permission { get; set; }
        public string CreateP { get; set; }
        public string ReadP { get; set; }
        public string UpdateP { get; set; }
        public string DeleteP { get; set; }
        public string ImportP { get; set; }
        public string ExportP { get; set; }
    }
}
