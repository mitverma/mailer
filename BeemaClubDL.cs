using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeInsuredDTO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using AOP;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using AutoMapper;
using Utilities;

namespace BeeInsuredBLDL
{
    public class BeemaClubDL
    {
        #region v3
        public ClaimSupport GetRegisteredClaim(int policyid, string username)
        {

            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("Get_RegisteredClaim"))
            {
                sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                sqldb.AddInParameter(objCMD, "@username", SqlDbType.VarChar, username);


                try
                {
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr = ds.Tables[0].CreateDataReader();
                    if (dr != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, ClaimSupport>();
                        List<ClaimSupport> claims = AutoMapper.Mapper.Map<IDataReader, List<ClaimSupport>>(dr);
                        return claims.FirstOrDefault();
                    }
                    else

                        return new ClaimSupport();






                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(ex);

                    return null;
                }
            }

        }

        private IList<T> GetDataFromDataTable<T>(DataSet dataSet, int index)
        {
            var table = dataSet.Tables[index];
            using (var reader = dataSet.CreateDataReader(table))
            {
                return Mapper.Map<IList<T>>(reader).ToList();
            }
        }

        public void UnRegisterClaim(int policyid, string username)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            using (DbCommand objCMD = objDB.GetStoredProcCommand("USP_UnRegisterClaim"))
            {
                sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                sqldb.AddInParameter(objCMD, "@username", SqlDbType.VarChar, username);


                try
                {
                    sqldb.ExecuteNonQuery(objCMD);



                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(ex);


                }
            }
        }

        internal static void InsertLapseReinstatement(LapseReinstatementDTO LapseDTO)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_AddEditPolicyLapseReinstatment"))
                {

                    sqldb.AddInParameter(objCMD, "@id", SqlDbType.Int, LapseDTO.Id);
                    sqldb.AddInParameter(objCMD, "@Policyid", SqlDbType.Int, LapseDTO.PolicyId);
                    sqldb.AddInParameter(objCMD, "@Pincode", SqlDbType.Int, LapseDTO.PinCode);
                    sqldb.AddInParameter(objCMD, "@LastAmountPaidDate", SqlDbType.DateTime, LapseDTO.LastAmountPaidDate);
                    sqldb.AddInParameter(objCMD, "@ModifiedDate", SqlDbType.DateTime, LapseDTO.ModifiedDate);
                    sqldb.AddInParameter(objCMD, "@Isactive", SqlDbType.Bit, LapseDTO.Isactive);
                    sqldb.AddInParameter(objCMD, "@ServiceCharge", SqlDbType.Decimal, LapseDTO.ServiceCharge);
                    sqldb.AddInParameter(objCMD, "@CreatedBy", SqlDbType.VarChar, LapseDTO.CreatedBy);
                    sqldb.AddInParameter(objCMD, "@ModifiedBy", SqlDbType.VarChar, LapseDTO.ModifiedBy);
                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.VarChar, LapseDTO.userid);
                    sqldb.AddInParameter(objCMD, "@status", SqlDbType.VarChar, LapseDTO.status);
                    sqldb.AddInParameter(objCMD, "@city", SqlDbType.VarChar, LapseDTO.city);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal List<LapseReinstatementDTO> GetLapseReinstatement(string userid, long policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<LapseReinstatementDTO> proposers = new List<LapseReinstatementDTO>();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetPolicyLapseReinstatment"))
                {


                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, LapseReinstatementDTO>();
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<LapseReinstatementDTO>>(dr1);

                    }
                }
                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        internal List<RegisteredPolicyDTO> GetLocker(string userid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<RegisteredPolicyDTO> proposers = new List<RegisteredPolicyDTO>();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetPolicyLocker"))
                {


                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);

                    }
                }
                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        internal List<RegisteredPolicyDTO> GetLapseReinstatementFirstRecord(string userid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<RegisteredPolicyDTO> proposers = new List<RegisteredPolicyDTO>();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetPolicyLapseReinstatment"))
                {


                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);

                    }
                }
                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }


        internal void SaveRenewalPickup(RenewalPickupDTO renewalPickupDto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_ADDEditRenewalPickup"))
                {
                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.BigInt, renewalPickupDto.Id);
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.BigInt, renewalPickupDto.UserId);
                    sqldb.AddInParameter(objCMD, "@PolicyId", SqlDbType.BigInt, renewalPickupDto.PolicyId);
                    sqldb.AddInParameter(objCMD, "@PremiumAmount", SqlDbType.Decimal, renewalPickupDto.PremiumAmount);
                    sqldb.AddInParameter(objCMD, "@Pincode", SqlDbType.VarChar, renewalPickupDto.Pincode);
                    sqldb.AddInParameter(objCMD, "@PickupDate", SqlDbType.DateTime, renewalPickupDto.PickupDate);
                    sqldb.AddInParameter(objCMD, "@PickupRate", SqlDbType.Decimal, renewalPickupDto.PickupRate);
                    sqldb.AddInParameter(objCMD, "@PickupSchedule", SqlDbType.VarChar, renewalPickupDto.PickupSchedule);
                    sqldb.AddInParameter(objCMD, "@Address", SqlDbType.VarChar, renewalPickupDto.Address);
                    sqldb.AddInParameter(objCMD, "@Landmark", SqlDbType.VarChar, renewalPickupDto.Landmark);
                    sqldb.AddInParameter(objCMD, "@City", SqlDbType.VarChar, renewalPickupDto.City);
                    sqldb.AddInParameter(objCMD, "@State", SqlDbType.VarChar, renewalPickupDto.State);
                    sqldb.AddInParameter(objCMD, "@Country", SqlDbType.VarChar, renewalPickupDto.Country);
                    sqldb.AddInParameter(objCMD, "@ContactNo", SqlDbType.VarChar, renewalPickupDto.ContactNo);
                    sqldb.AddInParameter(objCMD, "@Status", SqlDbType.VarChar, renewalPickupDto.Status);
                    sqldb.AddInParameter(objCMD, "@PickupBy", SqlDbType.VarChar, renewalPickupDto.PickupBy);
                    sqldb.AddInParameter(objCMD, "@Comments", SqlDbType.VarChar, renewalPickupDto.Comments);
                    sqldb.AddInParameter(objCMD, "@ChequeNo", SqlDbType.VarChar, renewalPickupDto.ChequeNo);
                    sqldb.AddInParameter(objCMD, "@ChequeDate", SqlDbType.DateTime, renewalPickupDto.ChequeDate);
                    sqldb.AddInParameter(objCMD, "@ChequeAmount", SqlDbType.Decimal, renewalPickupDto.ChequeAmount);
                    sqldb.AddInParameter(objCMD, "@BankName", SqlDbType.VarChar, renewalPickupDto.BankName);
                    sqldb.AddInParameter(objCMD, "@ContactPerson", SqlDbType.VarChar, renewalPickupDto.ContactPerson);
                    sqldb.AddInParameter(objCMD, "@Isactive", SqlDbType.Bit, renewalPickupDto.Isactive);


                    sqldb.AddInParameter(objCMD, "@CreatedDate", SqlDbType.DateTime, renewalPickupDto.CreatedDate);
                    sqldb.AddInParameter(objCMD, "@ModifiedDate", SqlDbType.DateTime, renewalPickupDto.ModifiedDate);
                    sqldb.AddInParameter(objCMD, "@ContactPersonAddress", SqlDbType.VarChar,
                        renewalPickupDto.ContactPersonAddress);
                    sqldb.AddInParameter(objCMD, "@CreatedBy", SqlDbType.VarChar, renewalPickupDto.CreatedBy);
                    sqldb.AddInParameter(objCMD, "@ModifiedBy", SqlDbType.VarChar, renewalPickupDto.ModifiedBy);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal List<RegisteredPolicyDTO> GetRegisteredPolicies(string userid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<RegisteredPolicyDTO> proposers = new List<RegisteredPolicyDTO>();

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRegisteredPolicies"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userid);
                    // sqldb.AddInParameter(objCMD, "@PolicyId", SqlDbType.Int, policyid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);
                    }
                }

                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }


        internal RegisterUserOtherDetailsDTO SaveRegisterUserOtherDetails(RegisterUserOtherDetailsDTO userdto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_SaveAndUpdateRegisterUserOtherDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, userdto.Id);
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userdto.UserId);
                    sqldb.AddInParameter(objCMD, "@SpouseAge", SqlDbType.Decimal, userdto.SpouseAge);
                    sqldb.AddInParameter(objCMD, "@FatherAge", SqlDbType.Decimal, userdto.FatherAge);
                    sqldb.AddInParameter(objCMD, "@MotherAge", SqlDbType.Decimal, userdto.MotherAge);
                    sqldb.AddInParameter(objCMD, "@Child1Age", SqlDbType.Decimal, userdto.Child1Age);
                    sqldb.AddInParameter(objCMD, "@Child2Age", SqlDbType.Decimal, userdto.Child2Age);
                    sqldb.AddInParameter(objCMD, "@Child3Age", SqlDbType.Decimal, userdto.Child3Age);
                    sqldb.AddInParameter(objCMD, "@Child4Age", SqlDbType.Decimal, userdto.Child4Age);
                    sqldb.AddInParameter(objCMD, "@Gender", SqlDbType.VarChar, userdto.Gender);
                    sqldb.AddInParameter(objCMD, "@MaritalStatus", SqlDbType.VarChar, userdto.MaritalStatus);
                    sqldb.AddInParameter(objCMD, "@Address", SqlDbType.VarChar, userdto.Address);
                    sqldb.AddInParameter(objCMD, "@SpouseName", SqlDbType.VarChar, userdto.SpouseName);
                    sqldb.AddInParameter(objCMD, "@FatherName", SqlDbType.VarChar, userdto.FatherName);
                    sqldb.AddInParameter(objCMD, "@MotherName", SqlDbType.VarChar, userdto.MotherName);
                    sqldb.AddInParameter(objCMD, "@Child1Name", SqlDbType.VarChar, userdto.Child1Name);
                    sqldb.AddInParameter(objCMD, "@Child1Relation", SqlDbType.VarChar, userdto.Child1Relation);
                    sqldb.AddInParameter(objCMD, "@Child2Name", SqlDbType.VarChar, userdto.Child2Name);
                    sqldb.AddInParameter(objCMD, "@Child2Relation", SqlDbType.VarChar, userdto.Child2Relation);
                    sqldb.AddInParameter(objCMD, "@Child3Name", SqlDbType.VarChar, userdto.Child3Name);
                    sqldb.AddInParameter(objCMD, "@Child3Relation", SqlDbType.VarChar, userdto.Child3Relation);
                    sqldb.AddInParameter(objCMD, "@Child4Name", SqlDbType.VarChar, userdto.Child4Name);
                    sqldb.AddInParameter(objCMD, "@Child4Relation", SqlDbType.VarChar, userdto.Child4Relation);
                    sqldb.AddInParameter(objCMD, "@SpouseDOB", SqlDbType.DateTime, userdto.SpouseDOB);
                    sqldb.AddInParameter(objCMD, "@FatherDOB", SqlDbType.DateTime, userdto.FatherDOB);
                    sqldb.AddInParameter(objCMD, "@MotherDOB", SqlDbType.DateTime, userdto.MotherDOB);
                    sqldb.AddInParameter(objCMD, "@Child1DOB", SqlDbType.DateTime, userdto.Child1DOB);
                    sqldb.AddInParameter(objCMD, "@Child2DOB", SqlDbType.DateTime, userdto.Child2DOB);
                    sqldb.AddInParameter(objCMD, "@Child3DOB", SqlDbType.DateTime, userdto.Child3DOB);
                    sqldb.AddInParameter(objCMD, "@Child4DOB", SqlDbType.DateTime, userdto.Child4DOB);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisterUserOtherDetailsDTO>();
                        RegisterUserOtherDetailsDTO proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisterUserOtherDetailsDTO>>(dr1).FirstOrDefault();
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        public void SaveLogintDeatils(DeviceInformation deviceInfo)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("AddUpdateLoginDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, deviceInfo.Id);
                    //sqldb.AddInParameter(objCMD, "@LogoutTime", SqlDbType.DateTime,);
                    sqldb.AddInParameter(objCMD, "@Devicetoken", SqlDbType.VarChar, deviceInfo.Uuid);
                    sqldb.AddInParameter(objCMD, "@Serial", SqlDbType.VarChar, deviceInfo.Serial);
                    sqldb.AddInParameter(objCMD, "@Platform", SqlDbType.VarChar, deviceInfo.Platform);
                    sqldb.AddInParameter(objCMD, "@CleverTapId", SqlDbType.VarChar, deviceInfo.CleverTapId);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
        }

        public void SaveLogoutDeatils(DeviceInformation deviceInfo)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("AddUpdateLogoutDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, deviceInfo.Id);
                    //sqldb.AddInParameter(objCMD, "@LogoutTime", SqlDbType.DateTime,);
                    sqldb.AddInParameter(objCMD, "@Devicetoken", SqlDbType.VarChar, deviceInfo.Uuid);
                    sqldb.AddInParameter(objCMD, "@Serial", SqlDbType.VarChar, deviceInfo.Serial);
                    sqldb.AddInParameter(objCMD, "@Platform", SqlDbType.VarChar, deviceInfo.Platform);
                    
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

        }

        internal AnnualTotalPremiumDetails GetTotalPremium(int userId)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetTotalPremium"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userId);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, AnnualTotalPremiumDetails>();
                        List<AnnualTotalPremiumDetails> proposers = AutoMapper.Mapper.Map<IDataReader, List<AnnualTotalPremiumDetails>>(dr1);

                        return proposers.FirstOrDefault();

                    }

                    
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

            return null;

        }

        internal void GetNextPremiumDueDate(RegisteredPolicyDTO policyDto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_UpdateAlreadyRenewedFlag"))
                {

                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyDto.Policyidd);
                    sqldb.AddInParameter(objCMD, "@paymode", SqlDbType.Int, policyDto.PayMode);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        List<RegisteredPolicyDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);
                        //return proposers.FirstOrDefault();
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

        }

        internal RegisterUserOtherDetailsDTO GetRegisterUserOtherDetails(RegisterUserOtherDetailsDTO userdto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRegisterUserOtherDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.BigInt, userdto.UserId);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisterUserOtherDetailsDTO>();
                        RegisterUserOtherDetailsDTO proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisterUserOtherDetailsDTO>>(dr1).FirstOrDefault();
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        internal List<RegisteredPolicyDTO> GetRenewalPickup(string userid, long policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<RegisteredPolicyDTO> proposers = new List<RegisteredPolicyDTO>();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRenewalPickup"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.BigInt, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.BigInt, policyid);
                    sqldb.AddInParameter(objCMD, "@id", SqlDbType.BigInt, 0);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        // List<ProposerDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<ProposerDTO>>(dr1);
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);

                    }
                }
                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }

        internal PolicyLockerDTO set_policyLocker(PolicyLockerDTO policylocker)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            //PolicyLockerDTO proposers = new PolicyLockerDTO();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_AddEditPolicyLocker"))
                {
                    sqldb.AddInParameter(objCMD, "@id", SqlDbType.Int, policylocker.Id);

                    sqldb.AddInParameter(objCMD, "@Policyid", SqlDbType.Int, policylocker.PolicyId);
                    sqldb.AddInParameter(objCMD, "@ModifiedDate", SqlDbType.DateTime, policylocker.ModifiedDate);
                    sqldb.AddInParameter(objCMD, "@NomineeName", SqlDbType.VarChar, policylocker.NomineeName);
                    sqldb.AddInParameter(objCMD, "@NomineeMobile", SqlDbType.VarChar, policylocker.NomineeMobile);
                    sqldb.AddInParameter(objCMD, "@NomineeRelationship", SqlDbType.VarChar, policylocker.NomineeRelationship);
                    sqldb.AddInParameter(objCMD, "@ContactName", SqlDbType.VarChar, policylocker.ContactName);
                    sqldb.AddInParameter(objCMD, "@ContactMobile", SqlDbType.VarChar, policylocker.ContactMobile);
                    sqldb.AddInParameter(objCMD, "@ContactRelationship", SqlDbType.VarChar, policylocker.ContactRelationship);
                    sqldb.AddInParameter(objCMD, "@CreatedBy", SqlDbType.VarChar, policylocker.CreatedBy);
                    sqldb.AddInParameter(objCMD, "@ModifiedBy", SqlDbType.VarChar, policylocker.ModifiedBy);
                    sqldb.AddInParameter(objCMD, "@isactive", SqlDbType.VarChar, policylocker.Isactive);
                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.VarChar, policylocker.UserId);
                    sqldb.AddInParameter(objCMD, "@ContactPersonEmailId", SqlDbType.VarChar, policylocker.ContactPersonEmailId);
                    sqldb.AddInParameter(objCMD, "@ContactPersonOtherMobileNo", SqlDbType.VarChar, policylocker.ContactPersonOtherMobileNo);
                    //sqldb.ExecuteNonQuery(objCMD);


                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        //proposers = AutoMapper.Mapper.Map<IDataReader, PolicyLockerDTO>(dr1);
                        List<PolicyLockerDTO> claims = AutoMapper.Mapper.Map<IDataReader, List<PolicyLockerDTO>>(dr1);
                        return claims.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        public RenewalPincodeDTO GetPincode(string pincode)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_Renewalpincode"))
                {

                    sqldb.AddInParameter(objCMD, "@pincode", SqlDbType.Int, pincode);


                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RenewalPincodeDTO>();
                        List<RenewalPincodeDTO> renewalpincode = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return renewalpincode.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }

        public LapseReinstatementDTO Getlapsefirstrecord(string userid, long policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetPolicyLapseReinstatment"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, LapseReinstatementDTO>();
                        List<LapseReinstatementDTO> LapseReinstatement = AutoMapper.Mapper.Map<IDataReader, List<LapseReinstatementDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return LapseReinstatement.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }

        public PolicyLockerDTO GetPolicyLocker(string userid, long policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetPolicyLocker"))
                {

                    sqldb.AddInParameter(objCMD, "@id", SqlDbType.Int, Convert.ToInt32(userid));
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        List<PolicyLockerDTO> PolicyLocker = AutoMapper.Mapper.Map<IDataReader, List<PolicyLockerDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return PolicyLocker.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }

        public PolicyLockerDTO GetPolicyContactInfo(string userid, long policyid, int contactid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetPolicyContactInfo"))
                {

                    sqldb.AddInParameter(objCMD, "@id", SqlDbType.Int, Convert.ToInt32(userid));
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, Convert.ToInt32(policyid));
                    sqldb.AddInParameter(objCMD, "@contactid", SqlDbType.Int, contactid);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        List<PolicyLockerDTO> PolicyLocker = AutoMapper.Mapper.Map<IDataReader, List<PolicyLockerDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return PolicyLocker.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }
        public RenewalPickupDTO GetRenewalPickupfirstrecord(string userid, long policyid, int id)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRenewalPickup"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                    sqldb.AddInParameter(objCMD, "@id", SqlDbType.Int, id);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    IDataReader dr1;
                    //Extract Table_HealthProposerDTO
                    if (id == 0)
                    {
                        dr1 = ds.Tables[0].CreateDataReader();
                    }
                    else
                    {
                        dr1 = ds.Tables[1].CreateDataReader();
                    }

                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RenewalPickupDTO>();
                        List<RenewalPickupDTO> RenewalPickup = AutoMapper.Mapper.Map<IDataReader, List<RenewalPickupDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return RenewalPickup.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }

        public RegisteredPolicyDTO GetRegisteredPolicyDetails(string userid, long policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            int userid1 = Convert.ToInt32(userid);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRegisteredPolicies"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userid1);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    IDataReader dr1;
                    //Extract Table_HealthProposerDTO
                    if (policyid == 0)
                    {
                        dr1 = ds.Tables[0].CreateDataReader();
                    }
                    else
                    {
                        dr1 = ds.Tables[1].CreateDataReader();
                    }

                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        List<RegisteredPolicyDTO> RegisteredPolicy = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return RegisteredPolicy.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }


        internal List<PolicyIsurerNameDTO> GetPolicyType(int id)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<PolicyIsurerNameDTO> proposers = new List<PolicyIsurerNameDTO>();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("MST_PolicyIsurerName"))
                {


                    sqldb.AddInParameter(objCMD, "@CategoryId", SqlDbType.Int, id);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    IDataReader dr1;
                    //Extract Table_HealthProposerDTO
                    if (id == 0)
                    {
                        dr1 = ds.Tables[0].CreateDataReader();
                    }
                    else
                    {
                        dr1 = ds.Tables[1].CreateDataReader();
                    }
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyIsurerNameDTO>();
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<PolicyIsurerNameDTO>>(dr1);

                    }
                }
                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        public int Insert_RegisteredPolicy(RegisteredPolicyDTO _RegisteredPolicy)
        {
            int id = 0;
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_AddUpdateRegisteredPolicy"))
                {

                    sqldb.AddInParameter(objCMD, "@PolicyId", SqlDbType.Int, _RegisteredPolicy.PolicyId);
                    sqldb.AddInParameter(objCMD, "@PolicyType", SqlDbType.Int, _RegisteredPolicy.PolicyType);
                    sqldb.AddInParameter(objCMD, "@Insurerid", SqlDbType.Int, _RegisteredPolicy.Insurerid);
                    sqldb.AddInParameter(objCMD, "@InsurerName", SqlDbType.VarChar, _RegisteredPolicy.Insurername);
                    sqldb.AddInParameter(objCMD, "@PremiumPayingTerm", SqlDbType.Int, _RegisteredPolicy.PremiumPayingTerm);
                    sqldb.AddInParameter(objCMD, "@PolicyTerm", SqlDbType.Int, _RegisteredPolicy.PolicyTerm);
                    sqldb.AddInParameter(objCMD, "@PremiumDueDate", SqlDbType.DateTime, _RegisteredPolicy.PremiumDueDate);
                    sqldb.AddInParameter(objCMD, "@MaturityDate", SqlDbType.DateTime, _RegisteredPolicy.MaturityDate);
                    sqldb.AddInParameter(objCMD, "@ModifiedDate", SqlDbType.DateTime, _RegisteredPolicy.ModifiedDate);
                    sqldb.AddInParameter(objCMD, "@Isactive", SqlDbType.Bit, _RegisteredPolicy.Isactive);
                    sqldb.AddInParameter(objCMD, "@Premium", SqlDbType.Decimal, _RegisteredPolicy.Premium);
                    sqldb.AddInParameter(objCMD, "@SumAssured", SqlDbType.Decimal, _RegisteredPolicy.SumAssured);
                    sqldb.AddInParameter(objCMD, "@PolicyFriendlyName", SqlDbType.VarChar, _RegisteredPolicy.PolicyFriendlyName);
                    sqldb.AddInParameter(objCMD, "@PolicyNickName", SqlDbType.VarChar, _RegisteredPolicy.PolicyNickName);
                    sqldb.AddInParameter(objCMD, "@PolicyNo", SqlDbType.VarChar, _RegisteredPolicy.PolicyNo);
                    sqldb.AddInParameter(objCMD, "@PayMode", SqlDbType.VarChar, _RegisteredPolicy.PayMode);
                    sqldb.AddInParameter(objCMD, "@CreatedBy", SqlDbType.VarChar, _RegisteredPolicy.CreatedBy);
                    sqldb.AddInParameter(objCMD, "@ModifiedBy", SqlDbType.VarChar, _RegisteredPolicy.ModifiedBy);
                    sqldb.AddInParameter(objCMD, "@Userid", SqlDbType.Int, Convert.ToInt32(_RegisteredPolicy.userid.ToString()));
                    sqldb.AddInParameter(objCMD, "@Name", SqlDbType.VarChar, _RegisteredPolicy.Name);
                    sqldb.AddInParameter(objCMD, "@EmailId", SqlDbType.VarChar, _RegisteredPolicy.EmailId);
                    sqldb.AddInParameter(objCMD, "@MobNo", SqlDbType.VarChar, _RegisteredPolicy.MobNo);
                    sqldb.AddInParameter(objCMD, "@MobNo1", SqlDbType.VarChar, _RegisteredPolicy.MobNo1);
                    sqldb.AddInParameter(objCMD, "@Relationship", SqlDbType.VarChar, _RegisteredPolicy.Relationship);
                    sqldb.AddInParameter(objCMD, "@DocName", SqlDbType.VarChar, _RegisteredPolicy.DocName);
                    sqldb.AddInParameter(objCMD, "@DocNameGuid", SqlDbType.VarChar, _RegisteredPolicy.DocNameGuid);
                    sqldb.AddInParameter(objCMD, "@Productid", SqlDbType.Int, _RegisteredPolicy.Productid);
                    sqldb.AddInParameter(objCMD, "@ProductName", SqlDbType.VarChar, _RegisteredPolicy.ProductName);
                    sqldb.AddInParameter(objCMD, "@PolicyCopy", SqlDbType.VarChar, _RegisteredPolicy.PolicyCopy);
                    sqldb.AddInParameter(objCMD, "@InceptionYear", SqlDbType.Int, _RegisteredPolicy.InceptionYear);
                    sqldb.AddInParameter(objCMD, "@PolicySource", SqlDbType.Int, _RegisteredPolicy.PolicySource);
                    // sqldb.ExecuteNonQuery(objCMD);


                    var recordId = sqldb.ExecuteScalar(objCMD);

                    int.TryParse(recordId.ToString(), out id);


                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return id;

        }

        internal void DeleteRenewalPickup(long recordid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_DeleteTRN_RenewalPickup"))
                {

                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, recordid);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal void DeletePolicyLocker(long recordid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_DeleteTRN_PolicyLocker"))
                {

                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, recordid);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal void DeleteLapseReinstatment(long recordid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_DeleteTRN_PolicyLapseReinstatment"))
                {

                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, recordid);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal void DeleteRenewalReminder(long recordid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_DeleteRH_RRRegisteredPolicy"))
                {

                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, recordid);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal void DeleteClaimSupport(long recordid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_DeleteRH_ClaimSupportDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, recordid);
                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        internal List<PolicyIsurerNameDTO> GetPolicyType(int id, string type)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<PolicyIsurerNameDTO> proposers = new List<PolicyIsurerNameDTO>();
            string spname = "MST_PolicyIsurerNameLocker";
            if (type == "LAPSE")
            {
                spname = "MST_PolicyIsurerNameLapse";
            }

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand(spname))
                {


                    sqldb.AddInParameter(objCMD, "@CategoryId", SqlDbType.Int, id);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    IDataReader dr1;
                    //Extract Table_HealthProposerDTO
                    if (id == 0)
                    {
                        dr1 = ds.Tables[0].CreateDataReader();
                    }
                    else
                    {
                        dr1 = ds.Tables[1].CreateDataReader();
                    }
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyIsurerNameDTO>();
                        proposers = AutoMapper.Mapper.Map<IDataReader, List<PolicyIsurerNameDTO>>(dr1);

                    }
                }
                return proposers;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        public void DeletePolicy(long userid, long policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_DeletePolicy"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@PolicyId", SqlDbType.Int, policyid);

                    sqldb.ExecuteNonQuery(objCMD);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

        }

        public void renewalreminder(RenewalReminderDTO renewalreminderDto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("RRRegisteredPolicy"))
                {

                    sqldb.AddInParameter(objCMD, "@RRPolicyId", SqlDbType.Int, renewalreminderDto.RRPolicyId);
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, renewalreminderDto.UserId);
                    sqldb.AddInParameter(objCMD, "@RRCategoryId", SqlDbType.Int, renewalreminderDto.RRCategoryId);
                    sqldb.AddInParameter(objCMD, "@RRInsId", SqlDbType.Int, renewalreminderDto.RRInsId);
                    sqldb.AddInParameter(objCMD, "@PayMode", SqlDbType.Int, renewalreminderDto.PayMode);
                    sqldb.AddInParameter(objCMD, "@PDD", SqlDbType.DateTime, renewalreminderDto.PDD);
                    sqldb.AddInParameter(objCMD, "@IsActive", SqlDbType.Bit, renewalreminderDto.IsActive);
                    sqldb.AddInParameter(objCMD, "@PolicyNumber", SqlDbType.VarChar, renewalreminderDto.PolicyNumber);
                    sqldb.AddInParameter(objCMD, "@PolicyNickName", SqlDbType.VarChar, renewalreminderDto.PolicyNickName);
                    sqldb.AddInParameter(objCMD, "@Premium", SqlDbType.Decimal, renewalreminderDto.Premium);
                    sqldb.AddInParameter(objCMD, "@SA", SqlDbType.VarChar, renewalreminderDto.SA);
                    sqldb.AddInParameter(objCMD, "@RemindOnMode", SqlDbType.VarChar, renewalreminderDto.RemindOnMode);
                    sqldb.AddInParameter(objCMD, "@DoneBy", SqlDbType.VarChar, renewalreminderDto.DoneBy);
                    sqldb.AddInParameter(objCMD, "@Opt", SqlDbType.VarChar, renewalreminderDto.Opt);
                    sqldb.AddInParameter(objCMD, "@RRMobile", SqlDbType.VarChar, renewalreminderDto.RRMobile);
                    sqldb.AddInParameter(objCMD, "@RREmail", SqlDbType.VarChar, renewalreminderDto.RREmail);

                    sqldb.ExecuteDataSet(objCMD);

                    ////Extract Table_HealthProposerDTO
                    //IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    //if (dr1 != null)
                    //{
                    //    AutoMapper.Mapper.CreateMap<IDataReader, RenewalReminderDTO>();
                    //    List<RenewalReminderDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalReminderDTO>>(dr1);

                    //}
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
        }

        public int newrenewalreminder(RenewalReminderDTO renewalreminderDto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            int id = 0;
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_SaveAndUpdateRenewalReminder"))   //RRRegisteredPolicy
                {

                    sqldb.AddInParameter(objCMD, "@RRPolicyId", SqlDbType.Int, renewalreminderDto.RRPolicyId);
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, renewalreminderDto.UserId);
                    sqldb.AddInParameter(objCMD, "@RRCategoryId", SqlDbType.Int, renewalreminderDto.RRCategoryId);
                    sqldb.AddInParameter(objCMD, "@RRInsId", SqlDbType.Int, renewalreminderDto.RRInsId);
                    sqldb.AddInParameter(objCMD, "@PayMode", SqlDbType.Int, renewalreminderDto.PayMode);
                    sqldb.AddInParameter(objCMD, "@PDD", SqlDbType.DateTime, renewalreminderDto.PDD);
                    sqldb.AddInParameter(objCMD, "@IsActive", SqlDbType.Bit, renewalreminderDto.IsActive);
                    sqldb.AddInParameter(objCMD, "@PolicyNumber", SqlDbType.VarChar, renewalreminderDto.PolicyNumber);
                    sqldb.AddInParameter(objCMD, "@PolicyNickName", SqlDbType.VarChar, renewalreminderDto.PolicyNickName);
                    sqldb.AddInParameter(objCMD, "@Premium", SqlDbType.Decimal, renewalreminderDto.Premium);
                    sqldb.AddInParameter(objCMD, "@SA", SqlDbType.VarChar, renewalreminderDto.SA);
                    sqldb.AddInParameter(objCMD, "@RemindOnMode", SqlDbType.VarChar, renewalreminderDto.RemindOnMode);
                    sqldb.AddInParameter(objCMD, "@DoneBy", SqlDbType.VarChar, renewalreminderDto.DoneBy);
                    sqldb.AddInParameter(objCMD, "@Opt", SqlDbType.VarChar, renewalreminderDto.Opt);
                    sqldb.AddInParameter(objCMD, "@RRMobile", SqlDbType.VarChar, renewalreminderDto.RRMobile);
                    sqldb.AddInParameter(objCMD, "@RREmail", SqlDbType.VarChar, renewalreminderDto.RREmail);

                    //sqldb.ExecuteDataSet(objCMD);

                    ////Extract Table_HealthProposerDTO
                    //IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    //if (dr1 != null)
                    //{
                    //    AutoMapper.Mapper.CreateMap<IDataReader, RenewalReminderDTO>();
                    //    List<RenewalReminderDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalReminderDTO>>(dr1);

                    //}
                    var recordId = sqldb.ExecuteScalar(objCMD);

                    int.TryParse(recordId.ToString(), out id);
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return id;
        }
        public ResultStatusDTO CanDoPickuo(int policyid)
        {
            ResultStatusDTO resultdto = new ResultStatusDTO();
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_CanDoPickup"))
                {

                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);

                    //DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    var dbresult = sqldb.ExecuteScalar(objCMD);
                    resultdto.ResultMessage = dbresult.ToString();

                    //Extract Table_HealthProposerDTO
                    //IDataReader dr1 = null;

                    //if (policyid > 0)
                    //{
                    //    dr1 = ds.Tables[0].CreateDataReader();
                    //}
                    //if (dr1 != null)
                    //{
                    //    AutoMapper.Mapper.CreateMap<IDataReader, ResultStatusDTO>();
                    //    List<ProposerDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<ProposerDTO>>(dr1);

                    //}

                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

            return resultdto;
        }

        public ResultStatusDTO CanDoLapse(int userid)
        {
            ResultStatusDTO resultdto = new ResultStatusDTO();
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_CanDoLapse"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userid);

                    //DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    var dbresult = sqldb.ExecuteScalar(objCMD);
                    resultdto.ResultMessage = dbresult.ToString();

                    //Extract Table_HealthProposerDTO
                    //IDataReader dr1 = null;

                    //if (policyid > 0)
                    //{
                    //    dr1 = ds.Tables[0].CreateDataReader();
                    //}
                    //if (dr1 != null)
                    //{
                    //    AutoMapper.Mapper.CreateMap<IDataReader, ResultStatusDTO>();
                    //    List<ProposerDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<ProposerDTO>>(dr1);

                    //}

                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }

            return resultdto;
        }

        public List<RegisteredPolicyDTO> GetRegisteredPoliciesCorner(int userid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            List<RegisteredPolicyDTO> policy = new List<RegisteredPolicyDTO>();
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRegisteredPoliciesCorner"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, @userid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        policy = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);

                        //if (policy.Count > 0)
                        //{
                        //    policy.ForEach(s =>
                        //    {
                        //        s.TotalPremium = policy.Select(c => c.Premium).Sum();

                        //    });
                        //}
                    }

                    IDataReader dr2 = ds.Tables[1].CreateDataReader();
                    if (dr2 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, EmergencyContactDTO>();
                        List<EmergencyContactDTO> emergencyContact = AutoMapper.Mapper.Map<IDataReader, List<EmergencyContactDTO>>(dr2);

                        policy.ForEach(x =>
                        {
                            x.Emergency = emergencyContact.Where(y => y.Userid == x.userid.ToInt() && y.Policyid == x.Policyidd).ToList();

                        });
                    }

                    IDataReader dr3 = ds.Tables[2].CreateDataReader();
                    if (dr3 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RenewalReminder>();
                        List<RenewalReminder> reminder = AutoMapper.Mapper.Map<IDataReader, List<RenewalReminder>>(dr3);
                        policy.ForEach(x =>
                        {
                            x.Details = reminder.Where(y => y.Userid == x.userid.ToInt() && y.PolicyId == x.Policyidd).FirstOrDefault();
                        });
                    }
                    if (ds.Tables.Count > 3)
                    {
                        IDataReader dr4 = ds.Tables[3].CreateDataReader();
                        if (dr4 != null)
                        {
                            AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                            policy.AddRange(AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr4));
                        }
                    }

                }
                return policy;
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        public RegisteredUser GetUserInfo(string userid)
        {
            int id = Convert.ToInt32(userid);
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetRegisteredUserInfo"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, id);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredUser>();
                        List<RegisteredUser> proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredUser>>(dr1);
                        return proposers.FirstOrDefault();
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;

        }


        public void DeletePolicyCorner(int policyid, int userid, int id, string option, string reasonid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_DeletePolicyCorner"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, id);
                    sqldb.AddInParameter(objCMD, "@option", SqlDbType.VarChar, option);
                    sqldb.AddInParameter(objCMD, "@reasonid", SqlDbType.VarChar, reasonid);
                    sqldb.ExecuteNonQuery(objCMD);

                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

        }

        internal int GetSearchEmailIdMobileNo(string EmailMob, int userid)
        {
            int cnt = 0;
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_SearchEmailIdMobileNo"))
                {

                    sqldb.AddInParameter(objCMD, "@searchdata", SqlDbType.VarChar, EmailMob);
                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);

                    object str = sqldb.ExecuteScalar(objCMD);
                    cnt = Convert.ToInt32(str);

                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return cnt;
        }

        internal UserDTO GetUserDetails(int uid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("Get_UserDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, uid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, UserDTO>();
                        UserDTO proposers = AutoMapper.Mapper.Map<IDataReader, List<UserDTO>>(dr1).FirstOrDefault();
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal UserDTO AddUpdate_RegisteredUser(UserDTO currentUser)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("UpdateUserDetails"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, currentUser.UserId);
                    sqldb.AddInParameter(objCMD, "@DOB", SqlDbType.DateTime, currentUser.DOB);
                    sqldb.AddInParameter(objCMD, "@Name", SqlDbType.VarChar, currentUser.FullName);
                    sqldb.AddInParameter(objCMD, "@City", SqlDbType.VarChar, currentUser.city);
                    sqldb.AddInParameter(objCMD, "@Mobile", SqlDbType.VarChar, currentUser.mobile);
                    sqldb.AddInParameter(objCMD, "@EmailID", SqlDbType.VarChar, currentUser.EmailID);
                    sqldb.AddInParameter(objCMD, "@Pincode", SqlDbType.VarChar, currentUser.Pincode);
                    sqldb.AddInParameter(objCMD, "@Gender", SqlDbType.VarChar, currentUser.Gender);
                    sqldb.AddInParameter(objCMD, "@Address", SqlDbType.VarChar, currentUser.Address);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[1].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, UserDTO>();
                        UserDTO proposers = AutoMapper.Mapper.Map<IDataReader, List<UserDTO>>(dr1).FirstOrDefault();
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal UserProfileDTO ChangePassword(int userid, string newpwd, string curpwd)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_ChangePassword"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, Convert.ToInt32(userid));
                    sqldb.AddInParameter(objCMD, "@Newpassword", SqlDbType.VarChar, newpwd);
                    sqldb.AddInParameter(objCMD, "@Oldpassword", SqlDbType.VarChar, curpwd);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, UserProfileDTO>();
                        UserProfileDTO proposers = AutoMapper.Mapper.Map<IDataReader, List<UserProfileDTO>>(dr1).FirstOrDefault();
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal List<PolicyLockerDTO> GetExistEmergencyContact(int userid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetExistEmergencyContact"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        List<PolicyLockerDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<PolicyLockerDTO>>(dr1);
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal List<PolicyLockerDTO> GetEmergencyContact(int userid, int policyid)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetEmergencyContact"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);


                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        List<PolicyLockerDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<PolicyLockerDTO>>(dr1);
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal UserDashboard GetUserDashboard(RegisteredUser userdto)
        {
            UserDashboard dashboard = new UserDashboard();
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetUserDashboard"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userdto.UserId);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, UserDashboard>();
                        dashboard = AutoMapper.Mapper.Map<IDataReader, List<UserDashboard>>(dr1).FirstOrDefault();
                    }
                    return dashboard;
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return dashboard;
        }

        internal RenewalReminderDTO GetRenewalDetailsByPolicyId(RegisteredPolicyDTO policyDto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetRenewalDetailsByPolicyId"))
                {

                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, Convert.ToInt32(policyDto.PolicyId));

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RenewalReminderDTO>();
                        RenewalReminderDTO proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalReminderDTO>>(dr1).FirstOrDefault();
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        public List<RegisteredPolicyDTO> GetUserPolicyByCategory(RegisteredPolicyDTO dto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetUserPolicyByCategory"))
                {

                    sqldb.AddInParameter(objCMD, "@categoryid", SqlDbType.Int, dto.PolicyType);
                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, dto.userid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        List<RegisteredPolicyDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal string SetUserAuthenticationToken(int userId, string Token)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_AddUpdateUserAuthToken"))
                {
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userId);
                    sqldb.AddInParameter(objCMD, "@Token", SqlDbType.VarChar, Token);

                    var authtoken = sqldb.ExecuteScalar(objCMD);
                    string token = "";
                    if (authtoken != null)
                        token = authtoken.ToString();
                    return token;
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        internal PolicyUploadDocumentDTO SaveAndUpdatePolicyUploadDocument(PolicyUploadDocumentDTO dto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_SaveAndUpdatePolicyUploadDocument"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, dto.UserId);
                    sqldb.AddInParameter(objCMD, "@PolicyId", SqlDbType.Int, dto.PolicyId);
                    sqldb.AddInParameter(objCMD, "@DocumentSeqNo", SqlDbType.Int, dto.DocumentSeqNo);
                    sqldb.AddInParameter(objCMD, "@CreatedDate", SqlDbType.DateTime, dto.CreatedDate);
                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.BigInt, dto.Id);
                    sqldb.AddInParameter(objCMD, "@DocumentUrl", SqlDbType.VarChar, dto.DocumentUrl);
                    sqldb.AddInParameter(objCMD, "@CreatedBy", SqlDbType.VarChar, dto.CreatedBy);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyUploadDocumentDTO>();
                        List<PolicyUploadDocumentDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<PolicyUploadDocumentDTO>>(dr1);
                        return proposers.FirstOrDefault();
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }



        internal List<PolicyLockerDTO> GetEmergencyContactForMobileApp(int userid, int policyid, int LoginThroughMobileApp)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetEmergencyContact"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                    sqldb.AddInParameter(objCMD, "@LoginTroughApp", SqlDbType.Int, LoginThroughMobileApp);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, PolicyLockerDTO>();
                        List<PolicyLockerDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<PolicyLockerDTO>>(dr1);
                        return proposers;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }
        #endregion

        #region v5
        public int Insert_RegisteredPolicy_V5(RegisteredPolicyDTO _RegisteredPolicy)
        {
            int id = 0;
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                //using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_AddUpdateRegisteredPolicy"))
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_AddUpdateRegisteredPolicy_TypeWise"))
                {

                    sqldb.AddInParameter(objCMD, "@PolicyId", SqlDbType.Int, _RegisteredPolicy.PolicyId);
                    sqldb.AddInParameter(objCMD, "@PolicyType", SqlDbType.Int, _RegisteredPolicy.PolicyType);
                    sqldb.AddInParameter(objCMD, "@Insurerid", SqlDbType.Int, _RegisteredPolicy.Insurerid);
                    sqldb.AddInParameter(objCMD, "@InsurerName", SqlDbType.VarChar, _RegisteredPolicy.Insurername);
                    sqldb.AddInParameter(objCMD, "@PremiumPayingTerm", SqlDbType.Int, _RegisteredPolicy.PremiumPayingTerm);
                    sqldb.AddInParameter(objCMD, "@PolicyTerm", SqlDbType.Int, _RegisteredPolicy.PolicyTerm);
                    sqldb.AddInParameter(objCMD, "@PremiumDueDate", SqlDbType.DateTime, _RegisteredPolicy.PremiumDueDate);
                    sqldb.AddInParameter(objCMD, "@MaturityDate", SqlDbType.DateTime, _RegisteredPolicy.MaturityDate);
                    sqldb.AddInParameter(objCMD, "@ModifiedDate", SqlDbType.DateTime, _RegisteredPolicy.ModifiedDate);
                    sqldb.AddInParameter(objCMD, "@Isactive", SqlDbType.Bit, _RegisteredPolicy.Isactive);
                    sqldb.AddInParameter(objCMD, "@Premium", SqlDbType.Decimal, _RegisteredPolicy.Premium);
                    sqldb.AddInParameter(objCMD, "@SumAssured", SqlDbType.Decimal, _RegisteredPolicy.SumAssured);
                    sqldb.AddInParameter(objCMD, "@PolicyFriendlyName", SqlDbType.VarChar, _RegisteredPolicy.PolicyFriendlyName);
                    sqldb.AddInParameter(objCMD, "@PolicyNickName", SqlDbType.VarChar, _RegisteredPolicy.PolicyNickName);
                    sqldb.AddInParameter(objCMD, "@PolicyNo", SqlDbType.VarChar, _RegisteredPolicy.PolicyNo);
                    sqldb.AddInParameter(objCMD, "@PayMode", SqlDbType.VarChar, _RegisteredPolicy.PayMode);
                    sqldb.AddInParameter(objCMD, "@CreatedBy", SqlDbType.VarChar, _RegisteredPolicy.CreatedBy);
                    sqldb.AddInParameter(objCMD, "@ModifiedBy", SqlDbType.VarChar, _RegisteredPolicy.ModifiedBy);
                    sqldb.AddInParameter(objCMD, "@Userid", SqlDbType.Int, Convert.ToInt32(_RegisteredPolicy.userid.ToString()));
                    sqldb.AddInParameter(objCMD, "@Name", SqlDbType.VarChar, _RegisteredPolicy.Name);
                    sqldb.AddInParameter(objCMD, "@EmailId", SqlDbType.VarChar, _RegisteredPolicy.EmailId);
                    sqldb.AddInParameter(objCMD, "@MobNo", SqlDbType.VarChar, _RegisteredPolicy.MobNo);
                    sqldb.AddInParameter(objCMD, "@MobNo1", SqlDbType.VarChar, _RegisteredPolicy.MobNo1);
                    sqldb.AddInParameter(objCMD, "@Relationship", SqlDbType.VarChar, _RegisteredPolicy.Relationship);
                    sqldb.AddInParameter(objCMD, "@DocName", SqlDbType.VarChar, _RegisteredPolicy.DocName);
                    sqldb.AddInParameter(objCMD, "@DocNameGuid", SqlDbType.VarChar, _RegisteredPolicy.DocNameGuid);
                    sqldb.AddInParameter(objCMD, "@Productid", SqlDbType.Int, _RegisteredPolicy.Productid);
                    sqldb.AddInParameter(objCMD, "@ProductName", SqlDbType.VarChar, _RegisteredPolicy.ProductName);
                    sqldb.AddInParameter(objCMD, "@PolicyCopy", SqlDbType.VarChar, _RegisteredPolicy.PolicyCopy);
                    sqldb.AddInParameter(objCMD, "@InceptionYear", SqlDbType.Int, _RegisteredPolicy.InceptionYear);
                    sqldb.AddInParameter(objCMD, "@PolicySource", SqlDbType.Int, _RegisteredPolicy.PolicySource);
                    sqldb.AddInParameter(objCMD, "@model", SqlDbType.VarChar, _RegisteredPolicy.MODEL);
                    sqldb.AddInParameter(objCMD, "@makemodel", SqlDbType.VarChar, _RegisteredPolicy.MAKE);
                    sqldb.AddInParameter(objCMD, "@make", SqlDbType.VarChar, _RegisteredPolicy.MAKE);
                    sqldb.AddInParameter(objCMD, "@RegNumber", SqlDbType.VarChar, _RegisteredPolicy.RegNumber);
                    sqldb.AddInParameter(objCMD, "@CurrentNCB", SqlDbType.VarChar, _RegisteredPolicy.CurrentNCB);
                    sqldb.AddInParameter(objCMD, "@Variant", SqlDbType.VarChar, _RegisteredPolicy.VARIENT);
                    sqldb.AddInParameter(objCMD, "@PurchaseDate", SqlDbType.VarChar, _RegisteredPolicy.PurchaseDate);
                    sqldb.AddInParameter(objCMD, "@Fueltype", SqlDbType.VarChar, _RegisteredPolicy.FuelType);
                    sqldb.AddInParameter(objCMD, "@VariantName", SqlDbType.VarChar, _RegisteredPolicy.VariantName);

                    sqldb.AddInParameter(objCMD, "@HealthPlanType", SqlDbType.VarChar, _RegisteredPolicy.HealthPlanType);
                    sqldb.AddInParameter(objCMD, "@CompositionType ", SqlDbType.VarChar, _RegisteredPolicy.familypattern);
                    sqldb.AddInParameter(objCMD, "@PolicyStartDate", SqlDbType.DateTime, _RegisteredPolicy.PolicyStartDate);
                    sqldb.AddInParameter(objCMD, "@PolicyEndDate", SqlDbType.DateTime, _RegisteredPolicy.PolicyEndDate);

                    sqldb.AddInParameter(objCMD, "@TravellerType", SqlDbType.VarChar, _RegisteredPolicy.TravellerType);

                    sqldb.AddInParameter(objCMD, "@TravelStartDate", SqlDbType.DateTime, _RegisteredPolicy.TravelStartDate);
                    sqldb.AddInParameter(objCMD, "@TravelEndDate", SqlDbType.DateTime, _RegisteredPolicy.TravelEndDate);

                    // sqldb.ExecuteNonQuery(objCMD);


                    var recordId = sqldb.ExecuteScalar(objCMD);

                    int.TryParse(recordId.ToString(), out id);


                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return id;

        }

        public int DeletePolicyCorner_V5(int policyid, int userid, int id, string option, string reasonid)
        {
            int ID =0; 
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_DeletePolicyCorner_TypeWise"))
                {

                    sqldb.AddInParameter(objCMD, "@userid", SqlDbType.Int, userid);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);
                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, id);
                    sqldb.AddInParameter(objCMD, "@option", SqlDbType.VarChar, option);
                    sqldb.AddInParameter(objCMD, "@reasonid", SqlDbType.VarChar, reasonid);
                    var recordId = sqldb.ExecuteScalar(objCMD);

                    int.TryParse(recordId.ToString(), out ID);

                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

            return ID;
        }

        public RegisteredPolicyDTO GetRegisteredPolicyDetails_V5(string userid, long policyid)
        {

            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            int userid1 = Convert.ToInt32(userid);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRegisteredPolicies_Typewise"))
                {

                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, userid1);
                    sqldb.AddInParameter(objCMD, "@policyid", SqlDbType.Int, policyid);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);
                    IDataReader dr1;
                    //Extract Table_HealthProposerDTO
                    if (policyid == 0)
                    {
                        dr1 = ds.Tables[0].CreateDataReader();
                    }
                    else
                    {
                        dr1 = ds.Tables[1].CreateDataReader();
                    }

                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, RegisteredPolicyDTO>();
                        List<RegisteredPolicyDTO> RegisteredPolicy = AutoMapper.Mapper.Map<IDataReader, List<RegisteredPolicyDTO>>(dr1);
                        // proposers = AutoMapper.Mapper.Map<IDataReader, List<RenewalPincodeDTO>>(dr1);
                        return RegisteredPolicy.FirstOrDefault();

                    }
                }

            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            finally
            {

            }
            return null;
        }

        public EmailVerification AddEmailVerification(EmailVerification emailVerification)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);

            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("AddUpdate_TRN_EmailVerification"))
                {
                    sqldb.AddInParameter(objCMD, "@Id", SqlDbType.Int, emailVerification.Id);
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.VarChar, emailVerification.UserId);
                    sqldb.AddInParameter(objCMD, "@Status", SqlDbType.VarChar, emailVerification.Status);
                    sqldb.AddInParameter(objCMD, "@EmailId", SqlDbType.VarChar, emailVerification.EmailId);
                    sqldb.AddInParameter(objCMD, "@CreatedDate", SqlDbType.DateTime, emailVerification.CreatedDate);
                    sqldb.AddInParameter(objCMD, "@Verifieddate", SqlDbType.DateTime, emailVerification.Verifieddate);
                    sqldb.AddInParameter(objCMD, "@IdKey", SqlDbType.VarChar, emailVerification.IdKey);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        Mapper.CreateMap<IDataReader, EmailVerification>();
                        EmailVerification resultset = Mapper.Map<IDataReader, List<EmailVerification>>(dr1).FirstOrDefault();
                        return resultset;
                    }

                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex);
            }
            return new EmailVerification();
        }


        public EmailVerification Get_EmailVerification(EmailVerification emailVerification)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetEmailVerification"))
                {

                    sqldb.AddInParameter(objCMD, "@IdKey", SqlDbType.VarChar, emailVerification.IdKey);
                    sqldb.AddInParameter(objCMD, "@UserId", SqlDbType.Int, emailVerification.UserId);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, EmailVerification>();
                        List<EmailVerification> proposers = AutoMapper.Mapper.Map<IDataReader, List<EmailVerification>>(dr1);
                        return proposers.SingleOrDefault();
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        #endregion
    }

    public class test
    {
        public int PolicyId { get; set; }
    }
}
