using Account.Model.DTO;
using Riok.Mapperly.Abstractions;

namespace Account.Model.Mappers;

[Mapper]
public static partial  class AccountMapper
{
  public static  partial AccountDTO AccountToAccountDto(Entities.Account account);
}