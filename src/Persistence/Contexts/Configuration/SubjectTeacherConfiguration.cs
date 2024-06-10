using Entities.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Contexts.Configuration
{
    public class SubjectTeacherConfiguration : IEntityTypeConfiguration<SubjectTeacher>
    {
        public void Configure(EntityTypeBuilder<SubjectTeacher> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id);
            entity.Property(e => e.IsMain).IsRequired();

            entity.HasOne(e => e.Teacher)
                .WithMany(e => e.SubjectTeachers)
                .HasForeignKey(e => e.TeacherId)
                .HasConstraintName("FK__SubjectTeacher__Teacher__TeacherId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            entity.HasOne(e => e.Subject)
                .WithMany(e => e.SubjectTeachers)
                .HasForeignKey(e => e.SubjectId)
                .HasConstraintName("FK__SubjectTeacher__Subject__SubjectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            //entity.HasData
            //    (
            //        new SubjectTeacher { Id = new Guid("67976e6a-3180-4792-b5c1-127b43180a08"), TeacherId = new Guid("4ecdfa98-3748-490a-94d9-d41372ede3af"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("0e373b10-9974-475b-b9a1-3ca7a7991df7"), TeacherId = new Guid("f2fb80b2-ca3e-4284-a914-0bb30afd4d7c"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("4d6f43f9-1a52-4f7d-b0bc-d14654686e6e"), TeacherId = new Guid("17179670-0ac5-417a-b499-55c4bcaeaecc"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("9b735b8d-5d56-4aee-97d6-3634dc6e9b51"), TeacherId = new Guid("7991b687-afc4-4995-b7ca-60eba94daa52"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("0e6c7274-0dcb-4739-82df-3cd23aa0ac3b"), TeacherId = new Guid("fc19384b-21a7-47e8-89cd-f569544c15c1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e3cb1bae-422b-459a-ae8d-37213e8577b7"), TeacherId = new Guid("a1a9f708-9a48-42fd-9308-3a106f9be31f"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("8a0a0fad-3d32-4fba-8e97-26887d82bb07"), TeacherId = new Guid("5e80e258-35bd-42b3-a6a5-cd46794565a9"), SubjectId = new Guid("c937c26a-de6b-4eb5-a9f3-58c3c37e17db"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("c95a461c-e277-4dc7-9de6-1d9bb092e198"), TeacherId = new Guid("1a4e58d0-5e19-42c2-b73f-ac0d7b6988f1"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("495bd9e1-3277-49e9-9f90-e79d95fdc129"), TeacherId = new Guid("14636c24-323b-459f-a824-0413df568c09"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("37d5e000-e911-4a2c-8646-cf35dd1b44b7"), TeacherId = new Guid("8a557738-6e34-43c5-9ec9-3b45ac4fdea5"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("47f39140-3009-4e58-a307-0dfb07f34594"), TeacherId = new Guid("cb45fd82-caf7-49b3-8afa-07ad739cba97"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("fd790306-ebc9-4ce0-a6cc-299f56c40dc9"), TeacherId = new Guid("2cebb584-9d66-4f80-83f1-67197b3dd52a"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("4cae5a36-9259-4fee-bb03-b0cdf6436e0e"), TeacherId = new Guid("6314c540-22ed-4084-926d-958a0eeedada"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("8c5f49bd-b2c0-4c8a-b72f-b92eb2e3d4aa"), TeacherId = new Guid("0e53a075-6f6f-472f-a356-ca31d8fbd1a7"), SubjectId = new Guid("36dc8e92-f9e5-4a1c-9b7a-11d5c1bef3f1"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("bfb5485d-72dd-4c48-b0c6-d4e45bf9ce4b"), TeacherId = new Guid("0c6c9e47-e7c5-4d30-9945-65672be86dfc"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("5bddfa10-730a-4e70-a37a-b6aaebdd303b"), TeacherId = new Guid("d07509af-9b19-46d4-a79c-018a1c247efb"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("feb3b86e-7b9a-4d83-96f9-a8e1bceb954b"), TeacherId = new Guid("fb1a4dad-e5a9-416c-aad1-f0c98e394e49"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("cb80397b-4697-4eb1-9364-e218b0996b8d"), TeacherId = new Guid("1e514fac-3089-4e09-9bf9-bea005578da5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("b246a8f4-f4a6-4963-ad08-06e5c5d331cd"), TeacherId = new Guid("fbcb8e24-1905-40fb-b49c-d27ff9ce20a2"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("4c13a264-d1a8-4df7-8c26-a208ddc79d0d"), TeacherId = new Guid("c27449f8-6eac-439a-901c-9b0b534ab428"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("dee0fc29-11c9-4116-9e59-43ba2c413721"), TeacherId = new Guid("9f09c5a3-7799-40ed-911c-c5161c05cabc"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("7b3ef9a3-1b97-44c4-a08a-7cbeebbca726"), TeacherId = new Guid("6749a629-6964-43f4-bc38-2173b963413c"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("fd9c7893-cec2-49da-8cd5-0fe21ad6471c"), TeacherId = new Guid("174ebe82-c04b-4996-b1b2-748a2b9ff8bc"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("0813fbc8-6f0f-4433-adf8-966bac3635d1"), TeacherId = new Guid("84d3bc96-0ad1-4269-8b40-207c2469e534"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("03cca51b-6fb0-41f7-8587-47d4695e19e0"), TeacherId = new Guid("475b148d-8c8f-42a3-beae-f81c4f01ae3a"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("7fb6eace-d15b-43c5-8a29-686dfdf81b14"), TeacherId = new Guid("f8fa22ac-0799-4bff-a34c-2674e70f012f"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("20d124cb-e3bf-4f6b-8da1-15edbb45d72c"), TeacherId = new Guid("647c4458-ab3e-45b5-9b71-7e109bfffdb5"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("51c31357-08cb-45e1-9d73-83525371ab95"), TeacherId = new Guid("e57f2e3e-6335-4fb4-bb4a-c12a73b5d7cd"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("9dce2563-9521-42d7-9bf9-69c8ef72566d"), TeacherId = new Guid("eb146dd1-1439-43b7-b4e2-420e58c733b0"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e9c90fa7-0d00-4e23-ae03-24a0182542dd"), TeacherId = new Guid("c85fbb73-6a36-4457-9477-0bb332b78d62"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("0c93cd6c-78f4-47d8-8154-c4206cdba1aa"), TeacherId = new Guid("c1f9b122-383b-42bd-9a04-10964679b550"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("234981d2-a2a8-4e61-a706-4bf94762788f"), TeacherId = new Guid("77277057-efa2-49b6-84e0-9ba7da852091"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e51a690b-9387-4341-9639-fb99adb97aed"), TeacherId = new Guid("a72bad70-0eda-45a1-9842-eb7e09527203"), SubjectId = new Guid("36dc8e92-f9e5-4a1c-9b7a-11d5c1bef3f1"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("da8cafee-1d74-40be-991b-49982e709595"), TeacherId = new Guid("7f392915-ee09-4dcf-91ec-17d94ec39563"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("45e61e03-e9a0-407a-803b-059791647571"), TeacherId = new Guid("4b3559b6-42ca-4f25-9192-d54903255462"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("396e4be9-12b8-47fe-9737-3ab32686efbf"), TeacherId = new Guid("e7dc5f6e-0671-48ec-a298-9bd7ab8ffbf1"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("c7455f44-3021-47b3-a10a-d03040064353"), TeacherId = new Guid("06583417-ef88-4a7f-a77e-ae2f494e5183"), SubjectId = new Guid("36dc8e92-f9e5-4a1c-9b7a-11d5c1bef3f1"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e445350a-7542-4b0c-bf65-e5787291309c"), TeacherId = new Guid("a90efdce-21e5-4a5c-93fd-d30721738db4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("55f1a857-b2e1-4a1f-afff-b61c94c4ea15"), TeacherId = new Guid("dae59f4a-9ee2-4995-8034-fd78c18d99bf"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e708bfc0-fe38-49ed-b0a1-aea380c9ded8"), TeacherId = new Guid("e889a534-4a57-41f1-8d12-f81358bd2b82"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("40a11199-f5a1-4031-99b4-2ed74e937727"), TeacherId = new Guid("e936ed8e-b9ff-448c-b641-2cede0b5c9da"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("40da26a7-186a-491c-996a-1bfa78df3186"), TeacherId = new Guid("e0c86ebc-287c-4b8f-8a34-c7752c537ece"), SubjectId = new Guid("93b939d4-ce8f-479c-a82f-ed2fe1278b31"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("dd1d009a-5c23-45a4-bd95-093e9ea2f03c"), TeacherId = new Guid("d44e77e2-00db-4f28-9364-bab2b01aeb7b"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("39445a14-d847-4087-b750-cccb726840e1"), TeacherId = new Guid("df6cc373-ae5c-4cce-b000-c48af3fe3e2b"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e97283ff-5282-421a-bae4-24b5dd1376fd"), TeacherId = new Guid("83d7b05b-69e5-4e88-8166-28dd81a004d9"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("fd254886-6839-44d6-931b-26b120238c8c"), TeacherId = new Guid("cee1fbee-342f-4089-8051-57f71c7d3903"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("e8fff96d-9623-424c-90b1-85337875b8de"), TeacherId = new Guid("00d40c59-0ba2-4231-9b6a-95345eea58bf"), SubjectId = new Guid("c30a4784-1394-45a7-a287-77cd660f4f02"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("2a9436e6-4ffc-427c-ae94-072a35f7932a"), TeacherId = new Guid("5aaac979-05dd-4e9a-8174-6d4885a40ce7"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("4bd73e03-e1f1-4abf-a239-4e281ae485b2"), TeacherId = new Guid("67174939-7dbf-44b7-a94d-42f3cc73457a"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("83e373d0-511b-43c2-932c-8aee95e4b5e9"), TeacherId = new Guid("f6a2d0e1-fcdc-4ea0-9c40-ffea09fd9490"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("05f4f92a-88f5-462a-ae2c-a7debd5d8f25"), TeacherId = new Guid("4bebc80b-f85d-42f6-910a-7c196354ee44"), SubjectId = new Guid("36dc8e92-f9e5-4a1c-9b7a-11d5c1bef3f1"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("817beed1-f8e7-4dbc-947b-93623108d984"), TeacherId = new Guid("33a1c99f-5f8f-400d-84eb-c2bfe2bf8b1e"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("36cf0c94-302b-4d61-b128-eeefbefdf818"), TeacherId = new Guid("307eb00b-0f9b-4f40-a955-57b4fe1b60e4"), SubjectId = new Guid("c8647f07-10d0-44c0-a9ed-fe0b9f2fca87"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("3d0c8ffc-955e-4df6-91d0-63a5d43dd3f9"), TeacherId = new Guid("5db317bf-1630-44c4-8b57-600abebf5c7c"), SubjectId = new Guid("a6e475a8-250b-4a2d-bf4e-d117144269ca"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("6d1fd7f8-7c73-4e75-a894-939d482f8b11"), TeacherId = new Guid("4986a2cc-8233-4a41-93ac-43e35e250c1a"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("cfd3fea3-60ea-4d21-8f7c-f3f88e27d196"), TeacherId = new Guid("81324152-45d1-4887-8e48-af431be4a30a"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("9431aaee-74e4-433c-8571-847e7fbdfa2f"), TeacherId = new Guid("d5551b7b-68e6-4b92-a881-c9c57cecb5a8"), SubjectId = new Guid("1289697a-0e3b-4f69-98ed-8b58f4f71c69"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("14b37c1c-9ce0-4b63-8a9c-9c3a5f47f3ac"), TeacherId = new Guid("8fdee9a8-c15d-41c0-97c7-36cd9f3a30bf"), SubjectId = new Guid("50250c21-75a1-4503-86c9-8f7e72503d4e"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("d14be636-cae0-460c-851f-12a211d2105d"), TeacherId = new Guid("bad1942a-795d-451e-802f-ce950719969c"), SubjectId = new Guid("0e8f864c-7433-4914-940c-1b2c4b2a7bdd"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("eeca7f6b-3146-4255-832c-ac9f2ea16b11"), TeacherId = new Guid("2de02504-74a4-4e8d-8100-81ece22d7db2"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("281055b3-4736-4581-995c-e9d1f4d90833"), TeacherId = new Guid("4f2b309b-1b10-4204-b627-7d376533ea15"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("fe6cb0ea-2836-404a-99ee-83c40b96c2b5"), TeacherId = new Guid("ba6997a0-4304-4c7d-9d8d-6e49c987f9d5"), SubjectId = new Guid("5e75c17c-9ab6-4284-a3df-b049bed4cf53"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("348ab0f4-0d6a-4a95-be00-e01002c4b4fb"), TeacherId = new Guid("4d72908d-8108-4828-88ad-e1797085b01b"), SubjectId = new Guid("578ea2f4-0f87-467a-8f5f-abb259f7d632"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("8dee3238-8b86-4e41-a8ac-dcbaaff34b54"), TeacherId = new Guid("b7adecee-f4d7-4a2d-8457-e5d27a427278"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("99ccaba9-13fe-4ed8-bee9-742ee64e6e03"), TeacherId = new Guid("5fe80a00-fa41-476c-a915-0238a70672cc"), SubjectId = new Guid("0978bcb4-3aea-458f-92f1-4f3c3ef5f8ba"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("26fd4962-8dbf-4b21-901f-aff2414305c5"), TeacherId = new Guid("d3c9cca0-bff7-4fdb-a664-fa55c082c14a"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("bc2e202a-6dc7-4de6-a9fe-b5f429977915"), TeacherId = new Guid("470ef95a-4b76-456f-8882-291493be8355"), SubjectId = new Guid("36dc8e92-f9e5-4a1c-9b7a-11d5c1bef3f1"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("f9ddf7b4-3406-4202-b256-00e98763a8de"), TeacherId = new Guid("29438207-e593-4766-a789-9f60feb184df"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true },
            //        new SubjectTeacher { Id = new Guid("19b3f068-303c-4aa1-841f-bcc6510bc9a5"), TeacherId = new Guid("c750afe2-d099-4ead-ba1b-691261b75d32"), SubjectId = new Guid("6b941b7e-f715-43a6-a7fc-dcc807fb37cb"), IsMain = true }
            //    );
        }
    }
}
