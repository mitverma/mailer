using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeInsuredDTO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using AOP;
using System.Configuration;
using System.Data.SqlClient;

namespace BeeInsuredBLDL
{
    public class LocatorDL
    {
        

        public List<GenericLocation> getState(StateQuery query)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("MST_LocationMap_GetStates"))
                {
                    if(query.InsurerName!= null)
                    sqldb.AddInParameter(objCMD, "@InsurerName", SqlDbType.VarChar,query.InsurerName);
                    if(query.Type!=null)
                    sqldb.AddInParameter(objCMD, "@Type", SqlDbType.VarChar, query.Type);
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, List<GenericLocation>>();
                        List<GenericLocation> locations = AutoMapper.Mapper.Map<IDataReader, List<GenericLocation>>(dr1);
                        return locations;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }
            return null;
        }

        public List<GenericLocation> getCities(CityQuery query)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("MST_LocationMap_GetCities"))
                {

                    sqldb.AddInParameter(objCMD, "@State", SqlDbType.VarChar, query.State);
                    if(query.Type != null)
                    sqldb.AddInParameter(objCMD, "@Type", SqlDbType.Int, Convert.ToInt32(query.Type));
                    if(query.InsurerName!=null)
                    sqldb.AddInParameter(objCMD, "@InsurerName", SqlDbType.VarChar, query.InsurerName);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, GenericLocation>();
                        List<GenericLocation> proposers = AutoMapper.Mapper.Map<IDataReader, List<GenericLocation>>(dr1);
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


        public List<GenericLocation> getPincodes(string State,string City)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("MST_LocationMap_GetPincodes"))
                {

                    sqldb.AddInParameter(objCMD, "@City", SqlDbType.VarChar, City);
                    sqldb.AddInParameter(objCMD, "@State", SqlDbType.VarChar, State);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, GenericLocation>();
                        List<GenericLocation> proposers = AutoMapper.Mapper.Map<IDataReader, List<GenericLocation>>(dr1);
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

        public List<LocatorDTO> getLocations(LocatorDTO locatorQuery)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetLocations"))
                {
                    sqldb.AddInParameter(objCMD, "@Type", SqlDbType.Int, Convert.ToInt16(locatorQuery.Type));
                    sqldb.AddInParameter(objCMD, "@State", SqlDbType.VarChar, locatorQuery.State==null?"": locatorQuery.State);
                    sqldb.AddInParameter(objCMD, "@City", SqlDbType.VarChar, locatorQuery.City==null ? "" : locatorQuery.City);
                    sqldb.AddInParameter(objCMD, "@Pincode", SqlDbType.VarChar, locatorQuery.Pincode ?? "");
                    sqldb.AddInParameter(objCMD, "@Area", SqlDbType.VarChar, locatorQuery.Area ?? "");
                    sqldb.AddInParameter(objCMD, "@InsurerName", SqlDbType.VarChar, locatorQuery.InsurerName ?? "");
                    sqldb.AddInParameter(objCMD, "@Latitude", SqlDbType.Decimal, locatorQuery.Lat);
                    sqldb.AddInParameter(objCMD, "@Longitude", SqlDbType.Decimal, locatorQuery.Lng);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, LocatorDTO>();
                        List<LocatorDTO> DTOs = AutoMapper.Mapper.Map<IDataReader, List<LocatorDTO>>(dr1);
                        return DTOs;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

            return null;
        }


        [Cache]
        public List<LocatorInsurer> getInsurers()
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetRH_InsurerMast"))
                {
                    
                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, LocatorInsurer>();
                        List<LocatorInsurer> DTOs = AutoMapper.Mapper.Map<IDataReader, List<LocatorInsurer>>(dr1);
                        return DTOs;
                    }
                }
            }
            catch (Exception excp)
            {
                Logging.Logger.Log(excp);
            }

            return null;
        }

    
        public void SaveLocation(dynamic obj)
        {
            var connString = "data source = WEBSERVER\\DEVSQL; Initial catalog = RH_BeeInsured; uid = dbuser; pwd = dbuser";
            SqlConnection con = new SqlConnection(connString);
            con.Open();
            SqlCommand updateCmd = new SqlCommand();
            updateCmd.CommandText = String.Format("Update RH_HospitalMast Set Lat = {0}, Lng = {1},Error = '{2}' where HospitalId = {3}", Convert.ToString(obj.Lat), Convert.ToString(obj.Lng),"OK",Convert.ToString(obj.Id));
            updateCmd.Connection = con;
            updateCmd.ExecuteNonQuery();
            con.Close();

        }


        public List<BranchPersonnelDTO> getBranchPersonnel(int branchId)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("APP_GetMST_BranchPersonnel"))
                {

                    sqldb.AddInParameter(objCMD, "@BranchId", SqlDbType.Int, branchId);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, BranchPersonnelDTO>();
                        List<BranchPersonnelDTO> proposers = AutoMapper.Mapper.Map<IDataReader, List<BranchPersonnelDTO>>(dr1);
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

        public InsurerDto GetInsurerName(InsurerDto insurerDto)
        {
            Database objDB = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            SqlDatabase sqldb = new SqlDatabase(objDB.ConnectionString);
            try
            {
                using (DbCommand objCMD = objDB.GetStoredProcCommand("App_GetInsurerNameByUrl"))
                {

                    sqldb.AddInParameter(objCMD, "@url", SqlDbType.VarChar, insurerDto.InsurerName);

                    DataSet ds = sqldb.ExecuteDataSet(objCMD);

                    //Extract Table_HealthProposerDTO
                    IDataReader dr1 = ds.Tables[0].CreateDataReader();
                    if (dr1 != null)
                    {
                        AutoMapper.Mapper.CreateMap<IDataReader, InsurerDto>();
                        InsurerDto proposers = AutoMapper.Mapper.Map<IDataReader, List<InsurerDto>>(dr1).FirstOrDefault();
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
    }
}
