using AccountManagement.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.Configurations;

public class AccountConfiguration: IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("account");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserName)
            .HasColumnName("username")
            .HasConversion(userName => userName.Value, value =>  UserName.Create(value).Value);
    }
}