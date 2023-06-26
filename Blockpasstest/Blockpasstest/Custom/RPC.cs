using System;
using System.Numerics;
using System.IO;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Runtime.InteropServices.ComTypes;
using Nethereum.Hex.HexConvertors.Extensions;
using NBitcoin;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Collections.Generic;

namespace Blockpasstest.Custom
{
    public class RPC
	{
        [FunctionOutput]
        public class Group: IFunctionOutputDTO
        {
            [Parameter("uint256", "id", 0)]
            public virtual uint Id { get; set; }
            [Parameter("string", "title", 1)]
            public virtual string Title { get; set; }
            [Parameter("string", "color", 2)]
            public virtual string Color { get; set; }
        }

        [FunctionOutput]
        public class GroupLength : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual uint GroupsAmount { get; set; }
        }

        [FunctionOutput]
        public class PassData : IFunctionOutputDTO
        {
            [Parameter("string", "title", 0)]
            public virtual string Title { get; set; }
            [Parameter("string", "data", 1)]
            public virtual string Data { get; set; }
        }

        [FunctionOutput]
        public class PasswordDTO : IFunctionOutputDTO
        {
            [Parameter("string", "title", 0)]
            public virtual string Title { get; set; }
            [Parameter("uint256", "groupId", 1)]
            public virtual uint GroupId { get; set; }
        }

        public class Password
        {
            public virtual string Title { get; set; }
            public virtual uint GroupId { get; set; }
            public virtual List<PassData> Data { get; set; }
        }

        [FunctionOutput]
        public class PasswordLength : IFunctionOutputDTO
        {
            [Parameter("uint", "length", 1)]
            public virtual uint PasswordsAmount { get; set; }
        }

        [FunctionOutput]
        public class ActivityLength : IFunctionOutputDTO
        {
            [Parameter("uint256", "length", 1)]
            public virtual uint Length { get; set; }
        }

        [FunctionOutput]
        public class UserActivity : IFunctionOutputDTO
        {
            [Parameter("string", "title", 0)]
            public virtual string Title { get; set; }
            [Parameter("string", "datetime", 0)]
            public virtual string Time { get; set; }
            [Parameter("string", "deviceType", 0)]
            public virtual string DeviceType { get; set; }
        }

        string groupСontractAddress = "0x31932FfbfA288AB8b04B646415a53f34c6805AE5";
        string groupABIjson = "[{\"anonymous\":false,\"inputs\":[{\"components\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"color\",\"type\":\"string\"}],\"indexed\":false,\"internalType\":\"struct UserGroups.Group\",\"name\":\"g\",\"type\":\"tuple\"}],\"name\":\"AddUserGrop\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"color\",\"type\":\"string\"}],\"name\":\"addGroupToUser\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"color\",\"type\":\"string\"}],\"name\":\"changeAllDataInExistingGroup\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"color\",\"type\":\"string\"}],\"name\":\"changeColorOfExistingGroup\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"}],\"name\":\"changeTitleOfExistingGroup\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"getUserGroupById\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"getUserGroups\",\"outputs\":[{\"components\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"color\",\"type\":\"string\"}],\"internalType\":\"struct UserGroups.Group[]\",\"name\":\"\",\"type\":\"tuple[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"getUserGroupsLength\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";

        string passwordsСontractAddress = "0xe8d264b785cCfCC455f60e5b1c4a2eEd9be33A39";
        string passwordsABIjson = "[{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"ChangeData\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"ChangeGroup\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"ChangeTitle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"PasswordAdded\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"uint256\",\"name\":\"groupId\",\"type\":\"uint256\"},{\"components\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"data\",\"type\":\"string\"}],\"internalType\":\"struct UserPasswords.PassData[]\",\"name\":\"data\",\"type\":\"tuple[]\"}],\"name\":\"addPassword\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"components\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"data\",\"type\":\"string\"}],\"internalType\":\"struct UserPasswords.PassData[]\",\"name\":\"data\",\"type\":\"tuple[]\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"changeData\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"groupId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"changeGroup\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"changeTitle\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"getUserPasswordById\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"uint256\",\"name\":\"groupId\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"dataId\",\"type\":\"uint256\"}],\"name\":\"getUserPasswordData\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"data\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"getUserPasswordDataLength\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"title\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"getUserPasswordsLenght\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"length\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";

        string activityСontractAddress = "0x72ba9DEc5CA0d8509E3CacE370592aB51F5d3033";
        string activityABIjson = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"datetime\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"deviceType\",\"type\":\"string\"}],\"name\":\"addActivity\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"getActivityById\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"title\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"datetime\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"deviceType\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"getActivityLength\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"length\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";

        public async Task<string> GetBlockNumber()
		{
            Web3 web3 = new Web3("http://31.131.26.13:8545");
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            return blockNumber.ToString();
		}

        public async Task addNewGroup(string name, string color)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(groupABIjson, groupСontractAddress);
            var addGroup = contract.GetFunction("addGroupToUser");
            var addGroupTX = addGroup.CreateTransactionInput(from: account.Address, new object[] { name, color });

            addGroupTX.From = account.Address;
            addGroupTX.To = groupСontractAddress;
            addGroupTX.GasPrice = new HexBigInteger(0);
            addGroupTX.Gas = await addGroup.EstimateGasAsync(account.Address, null, null, new object[] { name, color });

            App.Storage.AddDataInArrayByObjectKey<Group>("groups", "groups", new Group
            {
                Id = 5,
                Title = name,
                Color = color
            });
            var txnReceipt = await web3.Eth.TransactionManager.SendTransactionAsync(addGroupTX);
        }

        public async Task<List<Group>> getUserGroups()
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(groupABIjson, groupСontractAddress);
            var getGroups = contract.GetFunction("getUserGroupsLength");

            GroupLength res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<GroupLength>(account.Address);
            }
            catch (Exception e)
            {
                res = new GroupLength();
                Console.Write(e);
            }

            List<Group> groups = new List<Group>();
            if (res.GroupsAmount != 0)
            {
                for (uint i = 0; i < res.GroupsAmount; i++)
                {
                    Group g = await getUserGroup(i);
                    groups.Add(g);
                }
                return groups;
            }
            return groups;
        }

        public async Task<Group> getUserGroup(uint groupId)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(groupABIjson, groupСontractAddress);
            var getGroups = contract.GetFunction("getUserGroupById");

            Group res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<Group>(account.Address, groupId);
            } catch (Exception e)
            {
                res = new Group();
                Console.Write(e);
            }
            return res;
        }

        public async Task<List<Password>> getUserPasswords()
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(passwordsABIjson, passwordsСontractAddress);
            var getGroups = contract.GetFunction("getUserPasswordsLenght");

            PasswordLength res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<PasswordLength>(account.Address);
            }
            catch (Exception e)
            {
                res = new PasswordLength();
                Console.Write(e);
            }

            List<Password> passwords = new List<Password>();
            if (res.PasswordsAmount != 0)
            {
                for (uint i = 0; i < res.PasswordsAmount; i++)
                {
                    Password g = await getUserPassword(i);
                    passwords.Add(g);
                }
                return passwords;
            }
            return passwords;
        }

        public async Task<Password> getUserPassword (uint passId)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(passwordsABIjson, passwordsСontractAddress);
            var getGroups = contract.GetFunction("getUserPasswordById");

            PasswordDTO res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<PasswordDTO>(account.Address, passId);
            }
            catch (Exception e)
            {
                res = new PasswordDTO();
                Console.Write(e);
            }

            var dataLenght = contract.GetFunction("getUserPasswordDataLength");

            uint resLenght = 0;
            try
            {
                resLenght = await dataLenght.CallAsync<uint>(account.Address, passId);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            List<PassData> dataArr = new List<PassData>();

            for (uint i = 0; i < resLenght; i++)
            {
                PassData data = await getUserPasswordData(passId, i);
                dataArr.Add(data);
            }

            Password pass = new Password();
            pass.Title = res.Title;
            pass.GroupId = res.GroupId;
            pass.Data = dataArr;

            return pass;
        }

        public async Task<PassData> getUserPasswordData (uint passId, uint passDataId)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(passwordsABIjson, passwordsСontractAddress);
            var getGroups = contract.GetFunction("getUserPasswordData");

            PassData res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<PassData>(account.Address, passId, passDataId);
            }
            catch (Exception e)
            {
                res = new PassData();
                Console.Write(e);
            }
            return res;
        }

        public async Task addNewPassword(string title, string groupId, List<PassData> data)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(passwordsABIjson, passwordsСontractAddress);
            var addPassword = contract.GetFunction("addPassword");
            var addPasswordTX = addPassword.CreateTransactionInput(from: account.Address, new object[] { title, groupId, data });

            addPasswordTX.From = account.Address;
            addPasswordTX.To = passwordsСontractAddress;
            addPasswordTX.GasPrice = new HexBigInteger(0);
            addPasswordTX.Gas = await addPassword.EstimateGasAsync(account.Address, null, null, new object[] { title, groupId, data });

            uint.TryParse(groupId, out uint uintGroup);

            App.Storage.AddDataInArrayByObjectKey<Password>("passwords", "passwords", new Password
            {
                Title = title,
                GroupId = uintGroup,
                Data = data
            });
            var txnReceipt = await web3.Eth.TransactionManager.SendTransactionAsync(addPasswordTX);
        }

        public async Task addActivityRecord(string title)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(activityABIjson, activityСontractAddress);
            var addPassword = contract.GetFunction("addActivity");

            DateTimeOffset currentTime = DateTimeOffset.Now;
            string unixTimestamp = currentTime.ToUnixTimeSeconds().ToString();

            string os = "";
            string currentOs = Device.RuntimePlatform;

            switch (currentOs)
            {
                case Device.Android:
                    os = "Android";
                    break;
                case Device.iOS:
                    os = "iOS";
                    break;
                case Device.macOS:
                    os = "macOS";
                    break;
                case Device.UWP:
                    os = "Windows";
                    break;
                case Device.WPF:
                    os = "Windows";
                    break;
                default:
                    os = "Open Os";
                    break;
            }

            var addPasswordTX = addPassword.CreateTransactionInput(from: account.Address, new object[] { title, unixTimestamp, os });

            addPasswordTX.From = account.Address;
            addPasswordTX.To = activityСontractAddress;
            addPasswordTX.GasPrice = new HexBigInteger(0);
            addPasswordTX.Gas = await addPassword.EstimateGasAsync(account.Address, null, null, new object[] { title, unixTimestamp, os });
            
            var txnReceipt = await web3.Eth.TransactionManager.SendTransactionAsync(addPasswordTX);
        }

        public async Task<(List<UserActivity>, ActivityLength)> getUserActivity()
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(activityABIjson, activityСontractAddress);
            var getGroups = contract.GetFunction("getActivityLength");

            ActivityLength res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<ActivityLength>(account.Address);
            }
            catch (Exception e)
            {
                res = new ActivityLength();
                Console.Write(e);
            }

            List<UserActivity> activities = new List<UserActivity>();
            for (uint i = 0; i < res.Length; i++)
            {
                UserActivity data = await this.getUserActivityById(i);
                activities.Add(data);
            }

            return (activities, res);
        }

        public async Task<UserActivity> getUserActivityById(uint id)
        {
            string privateKey = Preferences.Get("__accountPrivate", "");
            int chainId = 211323;
            var account = new Nethereum.Web3.Accounts.Account(privateKey, chainId);

            var web3 = new Web3(account, "http://31.131.26.13:8545");
            web3.TransactionReceiptPolling.SetPollingRetryIntervalInMilliseconds(200);
            web3.TransactionManager.UseLegacyAsDefault = true;

            var contract = web3.Eth.GetContract(activityABIjson, activityСontractAddress);
            var getGroups = contract.GetFunction("getActivityById");

            UserActivity res;
            try
            {
                res = await getGroups.CallDeserializingToObjectAsync<UserActivity>(account.Address, id);
            }
            catch (Exception e)
            {
                res = new UserActivity();
                Console.Write(e);
            }
            return res;
        }
    }
}

