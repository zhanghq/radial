Radial Project
======
Radial project is a lightweight c# framework, which is designed to improve developers working efficiency, more convenient and efficient to build applications, It contains the following components:

### Dependency Injection ###
Radial contains an unified entrance class of Microsoft.Practices.Unity, easy to use, but many features in Radial depends on it

```csharp
Components.Container.RegisterType<IParam, XmlParam>();
```

### Logging ###
Logging component based on excellent Log4Net, Radial provides some convenient overloaded methods

```csharp
public void Debug(Exception exception)
public void Info(Exception exception, string format, params object[] args)
```

### Boot ###
Boot provides a scalable way to customize the system startup initialization，and support priority setting

```csharp
    public class GeneralBootTask : IBootTask
    {
        public virtual void Initialize()
        {
            //some works
        }

        public virtual void Start()
        {
            //on start 
        }

        public virtual void Stop()
        {
            //on stop 
        }
    }
```

###	Serialization ###
Includes Binary Serializer, Xml Serializer, and Json Serializer.

```csharp
string jtxt = JsonSerializer.Serialize(new { name ="link", age = 23 });

dynamic o = JsonSerializer.Deserialize(jtxt);
```

### Parameter ###
Parameter system is a important part of Radial, it allows you to save application settings in xml file or in database, and any changes will take effect immediately without reboot your application (eg. website) 

```csharp
Console.WriteLine(AppParam.GetValue("test1.level1.level1-1.level1-1-1"));
```

### Web & Network ###
Radial includes two levels of libraries for the web and network，first is the common code like：

- HttpWebClient: similar to the HttpClient class, but more convenient
- GeneralUpload: contains some methods for upload file operation
- SmtpMail: call SmtpClient to send email rapidly
- UserIdentity: a very useful wrapper class of HttpContext.Current.User.Identity in Web app
- HttpKits: http helper class，eg. Alert(Page page, string message)

the second level library is the extension of Asp.Net Mvc 3/4, such as: ActionResults, ModelBinders, Filters and Pagination

### Caching ###
Caching component has an abstract interface ICache, and some built-in concrete realizations. When using it, simply call their Facade entrance class

```csharp
            //primitive cache
            CacheStatic.Set("test0", 34.34, 100);
            Console.WriteLine(CacheStatic.Get<decimal>("test0"));

            //collection cache
            IList<Temp> list = new List<Temp>();
            IList<Temp> list2 = CacheStatic.Get<IList<Temp>>("test1");

            //object cache
            CacheStatic.Set("test2", new Temp2 { Name = "sdfsd" }, 100);
            Console.WriteLine(CacheStatic.Get("test2"));

            //enum cache
            CacheStatic.Set("test3",SerializeFormat.Json, 100);
            Console.WriteLine(CacheStatic.Get<SerializeFormat>("test3"));
```

### Persistence ###
Also a key aspect of Radial and also two libraries:
- Radial.Persist.Lite: a separate class library, the purpose is to enhance ADO.NET, and does not change the original data-tier architecture
- Radial.Persist.Nhs: based on NHibernate, support Unit Of Work, Repository, Multiple databases

```csharp
    public class User
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }

    class UserRepository : BasicRepository<User,int>
    {
        public UserRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
    
    public void FindAll()
    {
      using (IUnitOfWork uow = new NhUnitOfWork())
      {
        UserRepository userRepository = new UserRepository(uow);

        int total;

        IList<User> users = userRepository.FindAll(null, new OrderBySnippet<User>[] { new OrderBySnippet<User>(o => o.Id, false) }, 5, 1, out total);

        Assert.IsNotEmpty(users);
      }
    }
    
    public void FindByKeys()
    {
        int id1 = RandomCode.NewInstance.Next(1, int.MaxValue);
        int id2 = RandomCode.NewInstance.Next(1, int.MaxValue);

        using (IUnitOfWork uow = new NhUnitOfWork())
        {
            uow.RegisterNew<User>(new User { Id = id1, Name = "Name" });
            uow.RegisterNew<User>(new User { Id = id2, Name = "Name" });
            uow.Commit();
        }

        using (IUnitOfWork uow = new NhUnitOfWork())
        {
            UserRepository userRepository = new UserRepository(uow);

            Assert.AreEqual(2, userRepository.FindByKeys(new int[] { id1, id2 }).Count);
        }
    }
```

### Others ###
- Drawing: Captcha, ImageKits
- Security: CryptoProvider, X509CertificateProvider
- Checker: tools for check whether a condition is valid
- ExcelTools: import/export Execel files
- RangeValue: represents a range of value, minimum and maximum
- Validator: common used validation methods
- ......