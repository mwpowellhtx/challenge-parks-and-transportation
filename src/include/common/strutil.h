#ifndef COMMON_STRUTIL_HPP
#define COMMON_STRUTIL_HPP

#include <vector>
#include <string>

namespace challenges {

    struct split {

        enum split_option {
            none,
            remove_empty_entries,
        };

    private:

        split_option _option;

        void push_back(std::vector<std::string> & result, std::string const & s);

    public:

        split(split_option option = none);

        std::vector<std::string> operator()(std::string const & s, std::string const & d = " ");
    };
}

#endif //COMMON_STRUTIL_HPP
